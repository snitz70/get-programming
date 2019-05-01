type CustomerId = CustomerId of string

type ContactDetails = 
    | Address of string
    | Email of string
    | Telephone of string    

type Customer = 
    { CustomerId: CustomerId
      PrimaryContactDetails: ContactDetails
      SecondaryContactDetails: ContactDetails option }

let createCustomer customerId contactDetails secondaryDetails= 
    { CustomerId = customerId
      PrimaryContactDetails = contactDetails
      SecondaryContactDetails = secondaryDetails
      }

let customer = createCustomer (CustomerId "C-123") (Email "me@here.com") (Some (Telephone "542-1499"))

