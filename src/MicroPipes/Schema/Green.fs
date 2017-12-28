namespace MicroPipes.Schema.Green

open System
open MicroPipes.Schema
open NuGet.Versioning
open MicroPipes

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
    | MapType of NamedEntry list 
    | OneOfType of NamedEntry list
    | Wellknown of string option
    | Dummy

type TypeDeclaration =
    {
        TypeName: QualifiedIdentifier
        Body: TypeDefinition
        Type : Type option
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

