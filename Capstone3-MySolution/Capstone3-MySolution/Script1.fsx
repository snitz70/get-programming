#load @"C:\Users\brian snyder\source\repos\get-programming\Capstone2-MySolution\Capstone2-MySolution\Domain.fs"
#load @"C:\Users\brian snyder\source\repos\get-programming\Capstone2-MySolution\Capstone2-MySolution\Operations.fs"

open Domain
open Operations

let openingAccount = { Owner = { Name = "Brian" }; Balance = 0M; Id = System.Guid.Empty }

let account = 
    let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

    let isValidCommand command =
        command = 'd' || command = 'w' || command = 'x'

    let isStopCommand command = 
        command = 'x'

    let getAmount command = 
        match command with  
        | 'd' -> (command, 50M)
        | 'w' -> (command, 25M)
        | _ -> ('x', 0M)

    let processCommand account (command, amount) = 
        match command with 
        | 'd' -> account |> deposit amount
        | 'w' -> account |> withdraw amount
        | _ -> account

    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount

open System

let consoleCommands = seq {
    while true do
        Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
        yield Console.ReadKey().KeyChar
}

consoleCommands

