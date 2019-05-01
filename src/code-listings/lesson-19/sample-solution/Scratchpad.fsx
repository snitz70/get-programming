#load "Domain.fs"
#load "Operations.fs"

open Capstone3.Operations
open Capstone3.Domain
open System

// Listing 19.2 (Listing 19.1 is below!)
let isValidCommand = (Set [ 'w';'d';'x']).Contains
let isStopCommand = (=) 'x'
let getAmount command =
    if command = 'd' then 'd', 50M
    elif command = 'w' then 'w', 25M
    else command, 0M
let processCommand account (command, amount) =
    if command = 'd' then account |> deposit amount
    else account |> withdraw amount

// Listing 19.1
let openingAccount = { Owner = { Name = "Isaac" }; Balance = 0M; AccountId = Guid.Empty } 
let account =
    let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount

// Listing 19.3
let commands = seq {
    while true do
        Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
        yield Console.ReadKey().KeyChar }


#load "Auditing.fs"

open Capstone3.Auditing

// Test out create account from transaction history
let transactions =
    [ { Transaction.Accepted = false; Timestamp = DateTime.MinValue; Operation = "withdraw"; Amount = 10M }
      { Transaction.Accepted = true; Timestamp = DateTime.MinValue.AddSeconds 10.; Operation = "withdraw"; Amount = 10M }
      { Transaction.Accepted = true; Timestamp = DateTime.MinValue.AddSeconds 30.; Operation = "deposit"; Amount = 50M }
      { Transaction.Accepted = true; Timestamp = DateTime.MinValue.AddSeconds 50.; Operation = "withdraw"; Amount = 10M } ]

transactions = (transactions |> List.map (Transactions.serialize >> Transactions.deserialize))

let accountId = Guid.Empty
let owner = "Isaac"
let loadedAccount = loadAccount(owner, accountId, transactions)


type Guess =
    | Rock
    | Paper
    | Scisors

type Winner = 
    | Computer
    | Player
    | Tie

type Turn = (Guess * Guess) -> Winner

let playerGuess() = 
    Console.WriteLine ("Enter Rock, Paper, Scisors: ")    
    Console.ReadLine()

open Microsoft.FSharp.Reflection

let computerGuess() = 
    let guesses = typeof<Guess> |> FSharpType.GetUnionCases
    let random = Random()
    [for item in guesses -> item.Name].[random.Next(3)]

let game (playerGuess, computerGuess) = 
    match playerGuess with
    | Rock -> 
        match computerGuess with
        | Rock -> Tie
        | Paper -> Computer
        | Scisors -> Player
    | Scisors ->
        match computerGuess with 
        | Scisors -> Tie
        | Rock -> Computer
        | Paper -> Player
    | Paper ->
        match computerGuess with
        | Paper -> Tie
        | Rock -> Player
        | Scisors -> Computer



    

    
    




