namespace TestSchema.FSharp
open System
open MicroPipes

type VersionInsensivity =
    {
        Id : int option
        Name : string
    }
    
[<VersionRange("[2.0.0, 2.1.0]")>]    
type VersionSensivity() =
    member val Value : int = 0 with get, set 

[<VersionRange("[2.0.0,)")>]
type TestEnum =
    | One = 1us
    | [<VersionRange("[2.2.0,)")>] Two = 2us
    
    
type TestUnion =
    | First  of Version
    | Second of int 
    
