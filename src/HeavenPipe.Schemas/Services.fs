module HeavenPipe.Schemas.Services
open Literals
open Types

type EventDescription =
    {
        EventType : TypeReference
    } 
    
type CallArguments =
    | Single of TypeReference
    | Multiple of Map<Identifier, TypeReference> 

type CallDescription =
    {
        Arguments : CallArguments
        Result : TypeReference
    }

type EndpointDescription =
    | Event of EventDescription
    | Call of CallDescription

type ServiceDescription = Map<Identifier, EndpointDescription * Modifiers> * Modifiers

type SemanticVersion = Version of string

type SchemaDeclaration =
    {
        Namespace : QualifiedIdentifier
        Extensions : Set<QualifiedIdentifier>
        Version : SemanticVersion
        Types : Map<Identifier, TypeDescription>
        Services : Map<Identifier, ServiceDescription>        
    }