namespace MicroPipes.Schema.Green

open System
open MicroPipes.Schema
open NuGet.Versioning
open MicroPipes
open System.Reflection

type NamedTypeReference =
    {
        RefName : QualifiedIdentifier
        Type : Type option
    }

type TypeReference =
    | Unit
    | Basic of BasicType
    | Array of TypeReference
    | MayBe of TypeReference
    | Reference of NamedTypeReference
    | Tuple of TypeReference list

type NamedEntry =
    {
        MemberName : Identifier
        Index : int option
        TypeRef : TypeReference
        Summary: string option
        Extensions : Map<QualifiedIdentifier, Literal>
    }

type TypeDefinition =
    | EnumType of EnumType
    | MapType of BasedOn : NamedTypeReference option * NamedEntry list 
    | OneOfType of NamedEntry list
    | Wellknown of string option
    | Dummy

type Maked<'t when 't :> MemberInfo> =
    | Member of 't
    | UnionCase of FSharp.Reflection.UnionCaseInfo

module Maked =
    let fromMember<'t when 't :> MemberInfo> (a : 't) = Member (a :> MemberInfo)
    let fromType (t : Type) = Member t
    let fromCase (case: FSharp.Reflection.UnionCaseInfo) = UnionCase case


type TypeDeclaration =
    {
        TypeName: QualifiedIdentifier
        Body: TypeDefinition
        Type : Maked<Type> option
        Summary: string option
        Extensions : Map<QualifiedIdentifier, Literal>
    }

type Argument =
    {
        ArgName : Identifier
        Type : TypeReference
        Summary : string option
    }

type CallDefinition =
    {
        Arguments : Argument list
        Result : TypeReference * string option

    }

type EventDefinition =
    {
        EventType : TypeReference
    }

type EndpointDefinition =
    | Event of EventDefinition
    | Call of CallDefinition

type EndpointSchema =
    {
        EndpointName : QualifiedIdentifier
        Definition : EndpointDefinition 
        Summary : string option        
        Extensions : Map<QualifiedIdentifier, Literal>
    }





type ServiceSchema =
    {
        SchemaName : QualifiedIdentifier
        Version : SemanticVersion
        Types : TypeDeclaration list
        Endpoints: EndpointSchema list
    }

