module Capstone3.Operations

open System
open Capstone3.Domain


/// Withdraws an amount of an account (if there are sufficient funds)
let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

/// Deposits an amount into an account
let deposit amount account =
    { account with Balance = account.Balance + amount }

/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName audit operation amount (account:Account) =
    let transaction = { Amount = amount; Timestamp = DateTime.Now; Transaction = operationName; Result = false }
//    let audit = audit account.AccountId account.Owner.Name
//    audit (sprintf "%O: Performing a %s operation for £%M..." DateTime.UtcNow operationName amount)
    let updatedAccount = operation amount account
    let accountIsUnchanged = (updatedAccount = account)

    let transaction = 
        if accountIsUnchanged then transaction
        else { transaction with Result = true }

    audit account.AccountId account.Owner.Name transaction
    updatedAccount

let processTransaction account (transaction:Transaction) = 
    match transaction.Transaction with 
    | "d" ->  account |> deposit transaction.Amount
    | "w" -> account |> withdraw transaction.Amount
    | _ -> account

let loadAccount owner accountId transactions = 
    transactions
    |> List.sortByDescending (fun x -> x.Timestamp)
    |> List.fold processTransaction { AccountId = accountId ; Owner = { Name = owner }; Balance = 0M }