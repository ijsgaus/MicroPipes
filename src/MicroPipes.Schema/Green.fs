namespace MicroPipes.Schema.Green

open MicroPipes.Schema
open Aliases

type TypeReference =
    | UnitType
    | BasicType of BasicType
    | ArrayType of TypeReference
    | MayBeType of TypeReference
    | NamedType of QualifiedIdentifier
    | TupleType of TypeReference list

type NamedEntryGreen =
    {
        Name : Identifier
        Index : int option
        Kind : TypeReference
        Summary: string option
    }

type TypeDeclarationGreen =
    | EnumType of EnumType
    | MapType of NamedEntryGreen list 
    | OneOfType of NamedEntryGreen list
    | WellKnown 

type TypeGreen =
    {
        Body: TypeDeclarationGreen
        Summary: string option
    }

type ArgumentGreen =
    {
        Name : Identifier
        Type : TypeReference
        Summary : string option
    }

type CallGreen =
    {
        Arguments : ArgumentGreen list
        Result : TypeReference * string option
        Summary : string option
    }

type EndpointGreen =
    | Event of TypeReference
    | Call of CallGreen

type ServiceSchemaGreen =
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Types : QualifiedIdentifier * TypeGreen list
        Endpoints: QualifiedIdentifier * EndpointGreen list
    }