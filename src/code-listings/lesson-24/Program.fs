module Capstone4.Program

open System
open Capstone4.Domain
open Capstone4.Operations

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit
//let loadAccountOptional value = 
//    match value with
//    | Some value -> Some(Operations.loadAccount value)
//    | None -> None

let loadAccountOptional = Option.map Operations.loadAccount
let tryLoadAccountFromDisk = FileRepository.tryFindTransactionsOnDisk >> loadAccountOptional

[<AutoOpen>]
module CommandParsing =
    type Command = 
        | AccountCommand of BankOperation
        | Exit

    let tryParseCommand command = 
        match command with
        | 'd' -> Some (AccountCommand Deposit)
        | 'w' -> Some (AccountCommand Withdraw)
        | 'x' -> Some Exit
        | _ -> None

    let tryParseBankCommand command = 
        match command with
        | Exit -> None
        | AccountCommand op -> Some op

    //let isValidCommand cmd = [ 'd'; 'w'; 'x' ] |> List.contains cmd
    //let isValidCommand cmd = 
    //    cmd |> List.choose(fun x -> tryParseCommand x)

    //let isStopCommand = (=) 'x'

[<AutoOpen>]
module UserInput =
    let commands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
            Console.WriteLine() }
    
    //let getAmount command =
    //    Console.WriteLine()
    //    Console.Write "Enter Amount: "
    //    command, Console.ReadLine() |> Decimal.Parse

    let tryGetAmount command = 
        Console.WriteLine()
        Console.Write "Enter Amount: "
        let amount = Console.ReadLine() |> Decimal.TryParse
        match amount with
        | true, amount -> Some (command, amount)
        | false, _ -> None

[<EntryPoint>]
let main _ =
    let openingAccount =
        Console.Write "Please enter your name: "
        let owner = Console.ReadLine()

        match (tryLoadAccountFromDisk owner) with
        | Some account -> account
        | None -> 
            { Balance = 0M
              AccountId = Guid.NewGuid()
              Owner = { Name = owner } }

    //let openingAccount =
    //    Console.Write "Please enter your name: "
    //    Console.ReadLine() |> loadAccountFromDisk
    
    printfn "Current balance is £%M" openingAccount.Balance

    let processCommand account (command, amount) =
        printfn ""
        let account = 
            match command with 
                | Deposit -> account |> depositWithAudit amount
                | Withdraw -> account |> withdrawWithAudit amount

        //let account =
        //    if command = 'd' then account |> depositWithAudit amount
        //    else account |> withdrawWithAudit amount
        printfn "Current balance is £%M" account.Balance
        account

    let closingAccount =
        commands
        |> Seq.choose tryParseCommand
        |> Seq.takeWhile ((<>) Exit)
        |> Seq.choose tryParseBankCommand
        |> Seq.choose tryGetAmount
        |> Seq.fold processCommand openingAccount
    
    printfn ""
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0