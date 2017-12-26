namespace MicroPipes.Schema.Green

open System
open MicroPipes.Schema
open NuGet.Versioning
open MicroPipes.Schema.Literals

type NamedTypeReference =
    {
        Name : QualifiedIdentifier
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
        Name : Identifier
        Index : int option
        Type : TypeReference
        Summary: string option
    }

type TypeDefinition =
    | EnumType of EnumType
    | MapType of NamedEntry list 
    | OneOfType of NamedEntry list
    | Wellknown of string option

type TypeDeclaration =
    {
        Body: TypeDefinition
        Type : Type option
        Summary: string option
    }

type Argument =
    {
        Name : Identifier
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
        Name : QualifiedIdentifier
        Definition : EndpointDefinition 
        Summary : string option        
        Transports : Map<QualifiedIdentifier, Literal>
    }

type TypeSchema =
    {
        Name : QualifiedIdentifier
        Declaration : TypeDeclaration
    }



type ServiceSchema =
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Types : TypeSchema list
        Endpoints: EndpointSchema list
    }

