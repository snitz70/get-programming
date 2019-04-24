module Domain

type Customer = {
    Name: string
}

type Account = {
    Owner: Customer
    Balance: decimal
    Id: System.Guid
}


