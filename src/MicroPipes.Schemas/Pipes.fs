module MicroPipes.Schemas.Pipes
open System
open Literals
open Types
open FSharp.LanguageExt


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
        Summary : string option
    }

type EndpointDescription =
    | Event of TypeReference
    | Call of CallDescription

 
type FiberDescription =
    {
        Name : QualifiedIdentifier
        Version : SemanticVersion
        Summary : string option
        Imports : (QualifiedIdentifier * SemanticVersion) list
        Types : Map<Identifier, TypeDescription>
        Endpoint : EndpointDescription 
    }    

