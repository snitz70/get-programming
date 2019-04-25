// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open Domain
open Operations
open Auditing

let name = 
    Console.Write("Enter name: ")
    Console.ReadLine()

let openingBalance = 
    Console.WriteLine()
    Console.Write("Enter opening Balance: ")
    Console.ReadLine()

[<EntryPoint>]
let main argv = 
    let withdrawConsoleAudit = auditAs "withdraw" fileSystemAudit withdraw 
    let depositConsoleAudit = auditAs "deposit" fileSystemAudit deposit

    let mutable account = 
        { Id = Guid.NewGuid(); Owner = { Name = name }; Balance = Decimal.Parse(openingBalance) }

    while true do
        let action = 
            Console.Write ("(d)eposit, (w)ithdraw. or e(x)it: ")
            Console.ReadLine()  

        if action = "x" then Environment.Exit 0

        let amount = 
            Console.Write("Enter amount: ")
            Decimal.Parse(Console.ReadLine())
                    
        account <-
            if action = "d" then account |> depositConsoleAudit amount
            elif action = "w" then account |> withdrawConsoleAudit amount
            else account
    0
