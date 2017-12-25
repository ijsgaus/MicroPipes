namespace MicroPipes
open System

[<AttributeUsage(AttributeTargets.All)>]
type NameInSchemaAttribute(name: string) =
    inherit Attribute()
    member __.Name = name

