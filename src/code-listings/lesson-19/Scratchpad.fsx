#load "Domain.fs"
#load "Operations.fs"

open Capstone3.Operations
open Capstone3.Domain
open System

open System.IO
open System

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path

let private findAccountFolder owner =    
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name

let private buildPath(owner, accountId:Guid) = sprintf @"%s\%s_%O" accountsPath owner accountId

let serialized transaction = 
    sprintf "%O***%s***%M***%b"
        transaction.Timestamp
        transaction.Transaction
        transaction.Amount
        transaction.Result

/// Logs to the file system
let writeTransaction accountId owner (transaction:Transaction) =
    let path = buildPath(owner, accountId)    
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (DateTime.UtcNow.ToFileTimeUtc())
    File.WriteAllText(filePath, (serialized transaction))

writeTransaction (System.Guid.NewGuid()) "brian" { Amount = 10M; Transaction = "Deposit"; Timestamp = DateTime.Now; Result = true  }

let processTransaction account (transaction:Transaction) = 
    match transaction.Transaction with 
    | "d" ->  account |> deposit transaction.Amount
    | "w" -> account |> withdraw transaction.Amount
    | _ -> account

let processCommand account (command, amount) = 
    match command with 
    | 'd' -> account |> deposit amount
    | 'w' -> account |> withdraw amount
    | _ -> account

let loadAccount owner accountId transactions = 
    transactions
//    |> List.filter (fun x -> x.Result = true)
//    |> List.sortByDescending (fun x -> x.Timestamp)
    |> List.map(fun x -> (Convert.ToChar(x.Transaction), x.Amount))
    |> List.fold processCommand { AccountId = accountId ; Owner = { Name = owner }; Balance = 0M }

let deserialize (transaction:string) = 
    let fields = transaction.Split([|'*'|], StringSplitOptions.RemoveEmptyEntries)
    { Amount = Decimal.Parse(fields.[2]); Transaction = fields.[1]; Timestamp = DateTime.Parse(fields.[0]); Result = bool.Parse(fields.[3])}

deserialize "4/29/2019 3:10:37 PM***deposit***10***true"

loadAccount "Brian" (Guid.Parse("00000000-0000-0000-0000-000000000000")) 
    [ { Amount = 100M; Transaction = "d"; Timestamp = DateTime.Now; Result = true }
      { Amount = 110M; Transaction = "w"; Timestamp = DateTime.Now; Result = false }
      { Amount = 10M; Transaction = "w"; Timestamp = DateTime.Now; Result = true } ]