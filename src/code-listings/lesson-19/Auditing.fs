module Capstone3.Auditing

open Capstone3.Operations
open Capstone3.Domain

/// Logs to the console
let printTransaction (accountId:System.Guid) _ (transaction:Transaction) = printfn "Account %O: %O" accountId transaction

// Logs to both console and file system
let composedLogger = 
    let loggers =
        [ FileRepository.writeTransaction
          printTransaction ]
    fun accountId owner transaction ->
        loggers
        |> List.iter(fun logger -> logger accountId owner transaction)