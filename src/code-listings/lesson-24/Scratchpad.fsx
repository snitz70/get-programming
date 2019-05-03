#load "Domain.fs"
#load "Operations.fs"

open Capstone4.Operations
open Capstone4.Domain
open System
open System.IO

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

let accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path

Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" "Brian") |> Seq.toList

let buildPath(owner, accountId:Guid) = sprintf @"%s\%s_%O" accountsPath owner accountId

let loadTransactions (folder:string) =
    let owner, accountId =
        let parts = folder.Split '_'
        parts.[0], Guid.Parse parts.[1]
    owner, accountId, buildPath(owner, accountId)
                      |> Directory.EnumerateFiles
                      |> Seq.map (File.ReadAllText >> Transactions.deserialize)

let tryFindAccountFolder owner =
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner) |> Seq.toList
    match folders with
    | [] -> None
    | _ -> Some (DirectoryInfo(List.head.ToString()).Name)

let tryFindTransactionsOnDisk owner =
    let folder = tryFindAccountFolder owner
    match folder with 
    | Some folder -> Some (loadTransactions folder)
    | None -> None

tryFindTransactionsOnDisk  "brian"