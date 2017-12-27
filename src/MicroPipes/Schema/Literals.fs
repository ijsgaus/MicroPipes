namespace MicroPipes.Schema.Literals
open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open MicroPipes
open MicroPipes.Schema


type Ordinal =
    | U8 of byte
    | I8 of sbyte
    | U16 of uint16
    | I16 of int16
    | U32 of uint32
    | I32 of int
    | U64 of uint64
    | I64 of int64
    //#region Impicit operators
    [<SpecialName>]
    static member op_Implicit(value) = U8 value

    [<SpecialName>]
    static member op_Implicit(value) = I8 value

    [<SpecialName>]
    static member op_Implicit(value) = U16 value

    [<SpecialName>]
    static member op_Implicit(value) = I16 value

    [<SpecialName>]
    static member op_Implicit(value) = U32 value

    [<SpecialName>]
    static member op_Implicit(value) = I32 value

    [<SpecialName>]
    static member op_Implicit(value) = U64 value

    [<SpecialName>]
    static member op_Implicit(value) = I64 value
    //#endregion

type Float =
    | F32 of float32
    | F64 of float
    [<SpecialName>]
    static member op_Implicit(value) = F32 value

    [<SpecialName>]
    static member op_Implicit(value) = F64 value

type Basic =
    | Ordinal of Ordinal
    | Float of Float
    | String of string
    | Uuid of Guid
    | Bool of bool
    | DT of DateTime
    | DTO of DateTimeOffset
    | TS of TimeSpan
    | Id of QualifiedIdentifier
    | None
    [<SpecialName>]
    static member op_Implicit(value) = U8 value |> Ordinal
    [<SpecialName>]
    static member op_Implicit(value) = I8 value |> Ordinal

    [<SpecialName>]
    static member op_Implicit(value) = U16 value |> Ordinal
    [<SpecialName>]
    static member op_Implicit(value) = I16 value |> Ordinal

    [<SpecialName>]
    static member op_Implicit(value) = U32 value |> Ordinal
    [<SpecialName>]
    static member op_Implicit(value) = I32 value |> Ordinal
    [<SpecialName>]
    static member op_Implicit(value) = U64 value |> Ordinal
    [<SpecialName>]
    static member op_Implicit(value) = I64 value |> Ordinal
    [<SpecialName>]
    static member op_Implicit(value) = F32 value |> Float
    [<SpecialName>]
    static member op_Implicit(value) = F64 value |> Float

    [<SpecialName>]
    static member op_Implicit(value) = String value
    [<SpecialName>]
    static member op_Implicit(value) = Bool value

    [<SpecialName>]
    static member op_Implicit(value) = Uuid value
    [<SpecialName>]
    static member op_Implicit(value) = DT value
    [<SpecialName>]
    static member op_Implicit(value) = DTO value
    [<SpecialName>]
    static member op_Implicit(value) = TS value

    [<SpecialName>]
    static member op_Implicit(value) = Id value
     

and Literal =
    | Basic of Basic 
    | Array of Literal list
    | Map of Map<string, Literal>
    [<SpecialName>]
    static member op_Implicit(value) = value |> Seq.toList |> Array 
    [<SpecialName>]
    static member op_Implicit(value : IEnumerable<KeyValuePair<string, Literal>>) = 
        value 
            |> Seq.map (fun p -> p.Key, p.Value) 
            |> Map.ofSeq
            |> Map 

    [<SpecialName>]
    static member op_Implicit(value) = U8 value |> Ordinal |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = I8 value |> Ordinal |> Basic

    [<SpecialName>]
    static member op_Implicit(value) = U16 value |> Ordinal |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = I16 value |> Ordinal |> Basic

    [<SpecialName>]
    static member op_Implicit(value) = U32 value |> Ordinal |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = I32 value |> Ordinal |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = U64 value |> Ordinal |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = I64 value |> Ordinal |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = F32 value |> Float |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = F64 value |> Float |> Basic

    [<SpecialName>]
    static member op_Implicit(value) = String value |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = Bool value |> Basic

    [<SpecialName>]
    static member op_Implicit(value) = Uuid value |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = DT value |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = DTO value |> Basic
    [<SpecialName>]
    static member op_Implicit(value) = TS value |> Basic

    [<SpecialName>]
    static member op_Implicit(value) = Id value |> Basic
