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
     


type TypeDeclarationGreen =
    | EnumType of EnumTypeDescription
    | MapType of StructuredTypeDescriptionGreen 
    | OneOfType of StructuredTypeDescriptionGreen

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

type ServiceDescriptionGreen =
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Types : HashMap<QualifiedIdentifier, TypeDescriptionGreen>
        Endpoints: HashMap<QualifiedIdentifier, EndpointDescriptionGreen>
    }