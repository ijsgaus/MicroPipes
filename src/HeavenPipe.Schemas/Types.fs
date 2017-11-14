module HeavenPipe.Schemas.Types

open System
open Literals

 


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

type FieldedTypeDescription =
    | Indexed of Map<NameAndIndex, TypeReference * Modifiers>
    | Named of Map<Identifier, TypeReference * Modifiers>

type EnumTypeDescription =
    {
        IsFlag : bool
        Based : OrdinalType
        Values : Map<Identifier, OrdinalLiteral>
    }

type ComplexTypeDescription =
    | EnumType of EnumTypeDescription
    | MapType of FieldedTypeDescription
    | OneOfType of FieldedTypeDescription

type TypeDescription =
    {
        Body: ComplexTypeDescription
        Modifiers : Modifiers
    }


    
    
    


        
      
    
    

    
      