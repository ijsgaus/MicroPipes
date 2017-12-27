namespace MicroPipes.Schema
open System
open System.Linq
open System.Runtime.InteropServices
open System.Text
open System.Text.RegularExpressions
open MicroPipes

module Identifier =
    let parse id = Identifier.Parse id 
    let tryParse id = 
        match Identifier.TryParse id with
        | false, _ -> None
        | _, v -> Some v
    let create (ns : QualifiedIdentifier) name =
        QualifiedIdentifier(name, ns.Parts |> Seq.toArray)

    let parseQualified id = QualifiedIdentifier.Parse id
    let tryParseQualified id = 
        match QualifiedIdentifier.TryParse id with
        | false, _ -> None
        | _, v -> Some v
    let count (qid : QualifiedIdentifier) = qid.Count
    let name (qid : QualifiedIdentifier) = qid.Name
    let nameSpace (qid : QualifiedIdentifier) =
        match qid.Namespace with
        | null -> None
        | v -> Some v
        
        
 

