module Capstone3.Program

open System
open Capstone3.Domain
open Capstone3.Operations

[<EntryPoint>]
let main _ =
    let isValidCommand command =
        command = 'd' || command = 'w' || command = 'x'

    let isStopCommand command = 
        command = 'x'

    let getConsoleAmount command = 
        Console.WriteLine()
        Console.Write("Enter amount: ")
        command, Decimal.Parse(Console.ReadLine())

    let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
    let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit

    let processCommand account (command, amount) = 
        match command with 
        | 'd' -> account |> depositWithAudit amount
        | 'w' -> account |> withdrawWithAudit amount
        | _ -> account


    let name =
        Console.Write "Please enter your name: "
        Console.ReadLine()

    let openingAccount = { Owner = { Name = name }; Balance = 0M; AccountId = Guid.Empty } 

    let closingAccount =
        let consoleCommands = seq {
            while true do
                Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
                yield Console.ReadKey().KeyChar
        }

        consoleCommands
        |> Seq.filter isValidCommand
        |> Seq.takeWhile (not << isStopCommand)
        |> Seq.map getConsoleAmount
        |> Seq.fold processCommand openingAccount

    Console.Clear()
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0