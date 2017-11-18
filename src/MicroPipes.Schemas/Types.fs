module HeavenPipe.Schemas.Types

open System
open Literals

 
type OrdinalType =
    | U8 | I8 | U16 | I16 | U32 | I32 | U64 | I64
    
type BasicType =
    | Ordinal of OrdinalType
    | F32 | F64 | String | Uuid | DT | DTO | TS | Bool | Url

[<CustomEquality>]
[<CustomComparison>]
type NameAndIndex =
    {
        Name : Identifier
        Index : int
    }
    override __.Equals(other) =
        match other with
        | :? NameAndIndex as y -> __.Name = y.Name || __.Index = y.Index
        | _ -> false
    override __.GetHashCode() = __.Index.GetHashCode()
    interface IEquatable<NameAndIndex> with
        member __.Equals other =
            __.Name = other.Name || __.Index = other.Index
    interface IComparable<NameAndIndex> with
        member __.CompareTo other =
            if __.Name = other.Name || __.Index = other.Index 
                then 0
                else __.Index.CompareTo(other.Index)
    interface IComparable with
        member __.CompareTo other =
            match other with
            | :? NameAndIndex as y -> (__ :> IComparable<NameAndIndex>).CompareTo(y)
            | _ -> invalidArg "other" "cannot compare values of different types"


type TypeReference =
    | UnitType
    | BasicType of BasicType
    | ArrayType of TypeReference
    | MayBeType of TypeReference
    | NamedType of Identifier
    | TupleType of TypeReference list


type Modifiers = Map<QualifiedIdentifier, Literal>

type NamedEntry =
    {
        Kind : TypeReference
        Summary: string option
        Modifiers : Map<QualifiedIdentifier, Literal>
    }


type TypeWithDescription =
    | Indexed of Map<NameAndIndex, NamedEntry>
    | Named of Map<Identifier, NamedEntry>

type EnumField =
    {
        Value : OrdinalLiteral
        Summary : string option
    }

type EnumTypeDescription =
    {
        IsFlag : bool
        Based : OrdinalType
        Values : Map<Identifier, EnumField>
    }

type ComplexTypeDescription =
    | EnumType of EnumTypeDescription
    | MapType of TypeWithDescription 
    | OneOfType of TypeWithDescription

type TypeDescription =
    {
        Body: ComplexTypeDescription
        Modifiers : Modifiers
        Summary: string option
    }
    
type TypeLibrary = 
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Imports : (QualifiedIdentifier * SemanticVersion) list
        Types : TypeLibrary list
    }


    
    
    


        
      
    
    

    
      