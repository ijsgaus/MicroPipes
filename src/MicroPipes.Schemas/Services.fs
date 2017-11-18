module HeavenPipe.Schemas.Services
open Literals
open Types


type ArgumentDescription =
    {
        Name : Identifier
        Type : TypeReference
        Summary : string option
    }
    
type CallDescription =
    {
        Arguments : ArgumentDescription list
        Result : TypeReference * string option
    }

type EndpointDeclaration =
    | Event of TypeReference
    | Call of CallDescription
    
type EndpointDescription =
    {
        Name : Identifier
        Endpoint : EndpointDeclaration
        Modifiers : Modifiers
        Summary : string option
    }
    
type RouteNode =
    | GroupRoute of Literal * RouteNode list
    | Route of Literal * EndpointDeclaration 


type SemanticVersion = Version of string

type Protocol = Protocol of QualifiedIdentifier

type SchemaDeclaration =
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Types : Map<Identifier, TypeDescription>
        Cnannels : Map<Protocol, RouteNode list>        
    }