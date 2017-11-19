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

type NameAndIndex =
    {
        Name : Identifier
        Index : int
    }

[<Struct>]
type NameOrIndexEq =
    interface Eq<NameAndIndex> with
        member __.Equals (x, y) =
            StringComparer.InvariantCultureIgnoreCase.Equals(x.Name.ToString(), y.Name.ToString()) || x.Index = y.Index
        member __.GetHashCode x =
            x.Index.GetHashCode()

[<Struct>]    
type IdentifierIgnoreCaseEq =
    interface Eq<Identifier> with
        member __.Equals (x , y) =
            StringComparer.InvariantCultureIgnoreCase.Equals(x.ToString(), y.ToString())
        member __.GetHashCode x =
            StringComparer.InvariantCultureIgnoreCase.GetHashCode (x.ToString())

type StructuredTypeDescription =
    | Indexed of HashMap<NameOrIndexEq, NameAndIndex, NamedEntry>
    | Named of HashMap<IdentifierIgnoreCaseEq, Identifier, NamedEntry>

type EnumField =
    {
        Value : OrdinalLiteral
        Summary : string option
    }

type EnumTypeDescription =
    {
        IsFlag : bool
        Based : OrdinalType
        Values : HashMap<IdentifierIgnoreCaseEq, Identifier, EnumField>
    }

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


    
    
    


        
      
    
    

    
      