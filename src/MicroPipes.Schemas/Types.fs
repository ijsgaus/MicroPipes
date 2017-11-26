module MicroPipes.Schemas.Types

open System
open Literals
open FSharp.LanguageExt

 
type OrdinalType =
    | U8 | I8 | U16 | I16 | U32 | I32 | U64 | I64
    
type BasicType =
    | Ordinal of OrdinalType
    | F32 | F64 | String | Uuid | DT | DTO | TS | Bool | Url




type TypeReference =
    | UnitType
    | BasicType of BasicType
    | ArrayType of TypeReference
    | MayBeType of TypeReference
    | NamedType of QualifiedIdentifier
    | TupleType of TypeReference list


type NamedEntry =
    {
        Kind : TypeReference
        Summary: string option
    }

type StructuredTypeDescription =
    | Indexed of HashMap<NameOrIndexEq, NameAndIndex, NamedEntry>
    | Named of HashMap<IdentifierIgnoreCaseEq, Identifier, NamedEntry>

type EnumField<'t> =
    {
        Value : 't
        Summary : string option
    }

type EnumTypeDescription<'t> =
    {
        IsFlag : bool
        Values : HashMap<IdentifierIgnoreCaseEq, Identifier, EnumField<'t>>
    }
    
type EnumTypeDescription =
    | Enum8u  of EnumTypeDescription<byte>
    | Enum8   of EnumTypeDescription<sbyte>
    | Enum16  of EnumTypeDescription<int16>
    | Enum16u of EnumTypeDescription<uint16>
    | Enum32  of EnumTypeDescription<int32>
    | Enum32u of EnumTypeDescription<uint32>
    | Enum64  of EnumTypeDescription<int64>
    | Enum64u of EnumTypeDescription<uint64>
     


type TypeDeclaration =
    | EnumType of EnumTypeDescription
    | MapType of StructuredTypeDescription 
    | OneOfType of StructuredTypeDescription

type TypeDescription =
    {
        Body: TypeDeclaration
        Summary: string option
    }
    
type TypeLibrary = 
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Imports : (QualifiedIdentifier * SemanticVersion) list
        Types : HashMap<IdentifierIgnoreCaseEq, Identifier, TypeDescription>
    }


    
    
    


        
      
    
    

    
      