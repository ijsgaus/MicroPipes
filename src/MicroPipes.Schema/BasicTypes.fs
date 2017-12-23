namespace MicroPipes.Schema

open Aliases

type OrdinalType =
    | U8 | I8 | U16 | I16 | U32 | I32 | U64 | I64
    
type BasicType =
    | Ordinal of OrdinalType
    | F32 | F64 | String | Uuid | DT | DTO | TS | Bool | Url
    
type EnumField<'t> =
    {
        Value : 't
        Summary : string option
    }

type EnumType<'t> =
    {
        IsFlag : bool
        Values : HashMap<IdentifierIgnoreCaseEq, Identifier, EnumField<'t>>
    }
    
type EnumType =
    | Enum8u  of EnumType<byte>
    | Enum8   of EnumType<sbyte>
    | Enum16  of EnumType<int16>
    | Enum16u of EnumType<uint16>
    | Enum32  of EnumType<int32>
    | Enum32u of EnumType<uint32>
    | Enum64  of EnumType<int64>
    | Enum64u of EnumType<uint64>