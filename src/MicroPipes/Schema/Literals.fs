namespace MicroPipes.Schema
open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open MicroPipes


type OrdinalValue =
    | U8Value of byte
    | I8Value of sbyte
    | U16Value of uint16
    | I16Value of int16
    | U32Value of uint32
    | I32Value of int
    | U64Value of uint64
    | I64Value of int64
    //#region Impicit operators
    [<SpecialName>]
    static member op_Implicit(value) = U8Value value

    [<SpecialName>]
    static member op_Implicit(value) = I8Value value

    [<SpecialName>]
    static member op_Implicit(value) = U16Value value

    [<SpecialName>]
    static member op_Implicit(value) = I16Value value

    [<SpecialName>]
    static member op_Implicit(value) = U32Value value

    [<SpecialName>]
    static member op_Implicit(value) = I32Value value

    [<SpecialName>]
    static member op_Implicit(value) = U64Value value

    [<SpecialName>]
    static member op_Implicit(value) = I64Value value
    //#endregion

type FloatValue =
    | F32Value of float32
    | F64Value of float
    [<SpecialName>]
    static member op_Implicit(value) = F32Value value

    [<SpecialName>]
    static member op_Implicit(value) = F64Value value

type BasicValue =
    | OrdinalValue of OrdinalValue
    | FloatValue of FloatValue
    | StringValue of string
    | UuidValue of Guid
    | BoolValue of bool
    | DTValue of DateTime
    | DTOValue of DateTimeOffset
    | TSValue of TimeSpan
    | IdValue of QualifiedIdentifier
    | NoneValue
    [<SpecialName>]
    static member op_Implicit(value) = U8Value value |> OrdinalValue
    [<SpecialName>]
    static member op_Implicit(value) = I8Value value |> OrdinalValue

    [<SpecialName>]
    static member op_Implicit(value) = U16Value value |> OrdinalValue
    [<SpecialName>]
    static member op_Implicit(value) = I16Value value |> OrdinalValue

    [<SpecialName>]
    static member op_Implicit(value) = U32Value value |> OrdinalValue
    [<SpecialName>]
    static member op_Implicit(value) = I32Value value |> OrdinalValue
    [<SpecialName>]
    static member op_Implicit(value) = U64Value value |> OrdinalValue
    [<SpecialName>]
    static member op_Implicit(value) = I64Value value |> OrdinalValue
    [<SpecialName>]
    static member op_Implicit(value) = F32Value value |> FloatValue
    [<SpecialName>]
    static member op_Implicit(value) = F64Value value |> FloatValue

    [<SpecialName>]
    static member op_Implicit(value) = StringValue value
    [<SpecialName>]
    static member op_Implicit(value) = BoolValue value

    [<SpecialName>]
    static member op_Implicit(value) = UuidValue value
    [<SpecialName>]
    static member op_Implicit(value) = DTValue value
    [<SpecialName>]
    static member op_Implicit(value) = DTOValue value
    [<SpecialName>]
    static member op_Implicit(value) = TSValue value

    [<SpecialName>]
    static member op_Implicit(value) = IdValue value
     

and Literal =
    | BasicValue of BasicValue 
    | ArrayValue of Literal list
    | MapValue of Map<string, Literal>
    [<SpecialName>]
    static member op_Implicit(value) = value |> Seq.toList |> ArrayValue 
    [<SpecialName>]
    static member op_Implicit(value : IEnumerable<KeyValuePair<string, Literal>>) = 
        value 
            |> Seq.map (fun p -> p.Key, p.Value) 
            |> Map.ofSeq
            |> MapValue 

    [<SpecialName>]
    static member op_Implicit(value) = U8Value value |> OrdinalValue |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = I8Value value |> OrdinalValue |> BasicValue

    [<SpecialName>]
    static member op_Implicit(value) = U16Value value |> OrdinalValue |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = I16Value value |> OrdinalValue |> BasicValue

    [<SpecialName>]
    static member op_Implicit(value) = U32Value value |> OrdinalValue |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = I32Value value |> OrdinalValue |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = U64Value value |> OrdinalValue |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = I64Value value |> OrdinalValue |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = F32Value value |> FloatValue |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = F64Value value |> FloatValue |> BasicValue

    [<SpecialName>]
    static member op_Implicit(value) = StringValue value |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = BoolValue value |> BasicValue

    [<SpecialName>]
    static member op_Implicit(value) = UuidValue value |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = DTValue value |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = DTOValue value |> BasicValue
    [<SpecialName>]
    static member op_Implicit(value) = TSValue value |> BasicValue

    [<SpecialName>]
    static member op_Implicit(value) = IdValue value |> BasicValue
