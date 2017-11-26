namespace MicroPipes.Abstractions

[<AbstractClass>]
type Warranty internal () = 
    static member None = NoneWarranty()
    static member ToBroker = ToBrokerWarranty()
    static member ToClient = ToClientWarranty()
and
    ToClientWarranty internal() =
    inherit Warranty()
and 
    ToBrokerWarranty internal() =
    inherit ToClientWarranty()
and 
    NoneWarranty internal() =
    inherit ToBrokerWarranty()





        