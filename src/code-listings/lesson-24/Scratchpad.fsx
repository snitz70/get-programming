#load "Domain.fs"
#load "Operations.fs"

open Capstone4.Operations
open Capstone4.Domain
open System

type Command = 
    | Withdraw
    | Deposit
    | Exit

let tryParseCommand command = 
    match command with
    | 'd' -> Some Deposit
    | 'w' -> Some Withdraw
    | 'x' -> Some Exit
    | _ -> None

tryParseCommand 'x'

let isValidCommand cmd = 
    cmd |> List.choose(fun x -> tryParseCommand x)

isValidCommand ['d'; 'w'; 'f'; 'x']