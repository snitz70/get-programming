type Customer = {
    Name: string
}

type Account = {
    Owner: Customer
    Balance: decimal
    Id: System.Guid
}

let deposit amount account =
    { account with Balance = account.Balance + amount }

let withdraw amount account = 
    if amount <= account.Balance then   
        { account with Balance = account.Balance - amount }
    else 
        account

let account = { Owner = { Name = "Brian" }; Balance = 100.0M; Id = System.Guid.Empty }
account |> deposit 50M
account |> withdraw 110M

open System.IO

let fileSystemAudit account message = 
    let path = sprintf @"c:\temp\learnfs\capstone2\%s" account.Owner.Name
    let dir = Directory.CreateDirectory(path)

    let fileName = sprintf @"%s\%s.txt" (path) (account.Id.ToString())
    File.WriteAllText(fileName, message)

let console account message =
    printfn "Account %s: %s" (account.Id.ToString()) message

let auditAs (operationName: string) (audit:Account -> string -> unit)
    (operation:decimal -> Account -> Account) (amount:decimal)
    (account:Account) : Account =

    let newAccount = operation amount account

    if newAccount = account then    
        audit account (sprintf "%s failed" operationName)
        account
    else 
        audit account (sprintf "%s of $%.2f successfull" operationName amount)
        newAccount

let depositWithAuditing = auditAs "Deposit" console deposit
let withdrawWithAuditing = auditAs "Withdraw" console withdraw

account |> depositWithAuditing 100M
|> withdrawWithAuditing 75M

    

console account "testing"

fileSystemAudit account "testing the audit"