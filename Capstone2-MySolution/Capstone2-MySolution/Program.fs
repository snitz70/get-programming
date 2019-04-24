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

//let action = 
//    Console.Write ("(d)eposit, (w)ithdraw. or e(x)it: ")
//    Console.ReadLine()

[<EntryPoint>]
let main argv = 
    let withdrawConsoleAudit = auditAs "Withdraw" console withdraw 
    let depositConsoleAudit = auditAs "Deposit" console deposit

    let account = 
        { Id = Guid.NewGuid(); Owner = { Name = name }; Balance = Decimal.Parse(openingBalance) }

    while true do
        let action = 
            Console.Write ("(d)eposit, (w)ithdraw. or e(x)it: ")
            Console.ReadLine()      
            
        if action = "x" then Environment.Exit 0
        if action = "d" then account |> depositConsoleAudit 10M
        elif action = "w" then account |> withdrawConsoleAudit 10M
        else account


    printfn "%A" account
    0 // return an integer exit code
