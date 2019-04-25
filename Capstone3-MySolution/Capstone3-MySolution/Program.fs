// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Domain
open Operations
open Auditing
open System

let openingAccount = { Owner = { Name = "Brian" }; Balance = 0M; Id = System.Guid.Empty }


let isValidCommand command =
    command = 'd' || command = 'w' || command = 'x'

let isStopCommand command = 
    command = 'x'

let account = 
    //let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

    let printBalance account = 
        printfn "Current balance is $%.2f" account.Balance

    let consoleCommands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
    }

    let getConsoleAmount command = 
        Console.WriteLine()
        Console.Write("Enter amount: ")
        command, Decimal.Parse(Console.ReadLine())

    //let getAmount command = 
    //    match command with  
    //    | 'd' -> (command, 50M)
    //    | 'w' -> (command, 25M)
    //    | _ -> ('x', 0M)

    let depositWithConsoleAudit = auditAs "deposit" console deposit
    let withdrawConsoleAudit = auditAs "withdraw" console withdraw

    let processCommand account (command, amount) = 
        match command with 
        | 'd' -> account |> depositWithConsoleAudit amount
        | 'w' -> account |> withdrawConsoleAudit amount
        | _ -> account

    consoleCommands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getConsoleAmount
    |> Seq.fold processCommand openingAccount

[<EntryPoint>]
let main argv = 
    account

    0

