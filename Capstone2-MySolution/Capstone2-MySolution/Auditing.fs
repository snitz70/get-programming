module Auditing

open System.IO
open Domain

let fileSystemAudit account message = 
    let path = sprintf @"c:\temp\learnfs\capstone2\%s" account.Owner.Name
    let dir = Directory.CreateDirectory(path)

    let fileName = sprintf @"%s\%s.txt" (path) (account.Id.ToString())
    File.WriteAllText(fileName, message)

let console account message =
    printfn "Account %s: %s" (account.Id.ToString()) message

