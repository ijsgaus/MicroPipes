module MicroPipes.Schema.Literals
open System
open Aliases


type OrdinalLiteral =
    | U8Literal of byte
    | I8Literal of sbyte
    | U16Literal of uint16
    | I16Literal of int16
    | U32Literal of uint32
    | I32Literal of int32
    | U64Literal of uint64
    | I64Literal of int64


type BasicLiteral =
    | OrdinalLiteral of OrdinalLiteral
    | F32Literal of float32
    | F64Literal of float
    | StringLiteral of string
    | UuidLiteral of Guid
    | BoolLiteral of bool
    | DTLiteral of DateTime
    | DTOLiteral of DateTimeOffset
    | TSLiteral of TimeSpan
    | NoneLiteral

type MapLiteral =
    | Named of HashMap<IdentifierIgnoreCaseEq, Identifier, Literal>
    | Indexed of Map<int, Literal>
    | Botch of HashMap<NameOrIndexEq, NameAndIndex, Literal> 
and Literal =
    | Identifier of QualifiedIdentifier
    | Basic of BasicLiteral 
    | Array of Literal list
    | Map of MapLiteral


