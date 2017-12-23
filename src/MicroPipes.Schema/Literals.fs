module MicroPipes.Schema.Literals
open System
open Aliases

type Basic =
    | SignedOrdinal of int64
    | UnsignedOrdinal of uint64
    | Float of float
    | String of string
    | Uuid of Guid
    | Bool of bool
    | DT of DateTime
    | DTO of DateTimeOffset
    | TS of TimeSpan
    | Id of QualifiedIdentifier
    | None

and Literal =
    | Basic of Basic 
    | Array of Literal list
    | Map of HashMap<string, Literal>


