namespace MicroPipes.Schema.Green

open MicroPipes.Schema
open Aliases

type TypeReferenceGreen =
    | UnitType
    | BasicType of BasicType
    | ArrayType of TypeReferenceGreen
    | MayBeType of TypeReferenceGreen
    | NamedType of QualifiedIdentifier
    | TupleType of TypeReferenceGreen list

type NamedEntryGreen =
    {
        Kind : TypeReferenceGreen
        Summary: string option
    }

type StructuredTypeDescriptionGreen =
    | Indexed of HashMap<NameOrIndexEq, NameAndIndex, NamedEntryGreen>
    | Named of HashMap<IdentifierIgnoreCaseEq, Identifier, NamedEntryGreen>

     


type TypeDeclarationGreen =
    | EnumType of EnumTypeDescription
    | MapType of StructuredTypeDescriptionGreen 
    | OneOfType of StructuredTypeDescriptionGreen
    | WellKnown 

type TypeDescriptionGreen =
    {
        Body: TypeDeclarationGreen
        Summary: string option
    }

type ArgumentDescriptionGreen =
    {
        Name : Identifier
        Type : TypeReferenceGreen
        Summary : string option
    }

type CallDescriptionGreen =
    {
        Arguments : ArgumentDescriptionGreen list
        Result : TypeReferenceGreen * string option
        Summary : string option
    }

type EndpointDescriptionGreen =
    | Event of TypeReferenceGreen
    | Call of CallDescriptionGreen

type ServiceSchemaGreen =
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Types : HashMap<QualifiedIdentifier, TypeDescriptionGreen>
        Endpoints: HashMap<QualifiedIdentifier, EndpointDescriptionGreen>
    }