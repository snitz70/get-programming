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

    audit account (sprintf "Performing %s operation for $%.2f... \r\n" operationName amount )
    
    let newAccount = operation amount account

    if newAccount = account then    
        audit account "Transaction rejected!\r\n"
        account
    else 
        audit account (sprintf "Transaction accepted!  Balance is now $%.2f\r\n" newAccount.Balance)
        newAccount

