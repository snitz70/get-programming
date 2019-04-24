module Operations

open Domain

let deposit amount account =
    { account with Balance = account.Balance + amount }

let withdraw amount account = 
    if amount <= account.Balance then   
        { account with Balance = account.Balance - amount }
    else 
        account

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

