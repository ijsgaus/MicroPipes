module HeavenPipe.Schemas.Services
open Literals
open Types


type ArgumentDescription =
    {
        Name : Identifier
        Type : TypeReference
    }
    
type CallDescription =
    {
        Arguments : ArgumentDescription list
        Result : TypeReference
    }

type EndpointDeclaration =
    | Event of TypeReference
    | Call of CallDescription
    
type EndpointDescription =
    {
        Name : Identifier
        Endpoint : EndpointDeclaration
        Modifiers : Modifiers
    }
    
type RouteNode =
    | GroupRoute of Literal * RouteNode list
    | Route of Literal * EndpointDeclaration 


type SemanticVersion = Version of string

type AddressOrAlias =
    | Uri of UrlLiteral
    | Alias of Identifier
    | Default
    
type Protocol =
    {
        Name : QualifiedIdentifier
        Address : AddressOrAlias
    }


type SchemaDeclaration =
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Types : Map<Identifier, TypeDescription>
        Cnannels : Map<Protocol, RouteNode list>        
    }