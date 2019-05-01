namespace Capstone3.Domain

open System

type Customer = { Name : string }
type Account = { AccountId : Guid; Owner : Customer; Balance : decimal }
type Transaction = { Amount: decimal; Transaction: string; Timestamp: DateTime; Result: bool }

