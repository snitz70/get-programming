// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Domain
open Operations
open Auditing
open System

let name = 
    Console.Write("Enter name: ")
    Console.ReadLine()

let depositWithConsoleAudit = auditAs "deposit" console deposit
let withdrawConsoleAudit = auditAs "withdraw" console withdraw

let isValidCommand command =
    command = 'd' || command = 'w' || command = 'x'

let isStopCommand command = 
    command = 'x'

let getConsoleAmount command = 
    Console.WriteLine()
    Console.Write("Enter amount: ")
    command, Decimal.Parse(Console.ReadLine())

let processCommand account (command, amount) = 
    match command with 
    | 'd' -> account |> depositWithConsoleAudit amount
    | 'w' -> account |> withdrawConsoleAudit amount
    | _ -> account


[<EntryPoint>]
let main argv = 
    let openingAccount = { Owner = { Name = name }; Balance = 0M; Id = Guid.NewGuid() }

    let account = 
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

    let endingAccount = account
    0

