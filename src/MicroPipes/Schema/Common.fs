namespace MicroPipes.Schema
open System
open System.Linq
open System.Runtime.InteropServices
open System.Text
open System.Text.RegularExpressions

[<CustomEquality>]
[<CustomComparison>]
type Identifier = 
        private | Identifier of string
        static member private regEx = Regex("^[A-Za-z][A-Za-z0-9_]*$")
        static member TryParse(str, [<Out>] id : Identifier byref) =
            id <- Unchecked.defaultof<Identifier>
            match Identifier.regEx.IsMatch(str) with
            | true -> id <- Identifier str; true
            | _ -> false
        static member Parse id =
            match Identifier.TryParse id with
            | false, _ -> sprintf "Invalid identifier '%s'" id |> invalidArg "id"
            | _, v -> v
        override __.ToString () = let (Identifier s) = __ in s
        override __.Equals o =
            match o with
            | null -> false
            | :? Identifier as i -> (__ :> IEquatable<Identifier>).Equals(i)
            | _ -> false
        override __.GetHashCode() = StringComparer.InvariantCultureIgnoreCase.GetHashCode(__.ToString())
        interface IEquatable<Identifier> with
            member __.Equals other = 
                StringComparer.InvariantCultureIgnoreCase.Equals(__.ToString(), other.ToString())
        interface IComparable with
            member __.CompareTo other = 
                match other with
                | null -> 1
                | :? Identifier as i -> StringComparer.InvariantCultureIgnoreCase.Compare(__.ToString(), i.ToString())
                | _ -> invalidOp "Cannot compare Identifier with other type"

type QualifiedIdentifier =
    private | Qualified of Identifier list  
    override __.ToString() =
        let (Qualified q) = __
        let sb = StringBuilder()
        let folder (a : StringBuilder) (id:Identifier) = 
            if a.Length = 0 then a.Append(id) else a.Append(".").Append(id)
        (q |> Seq.fold folder sb).ToString()
    static member Create (head, [<ParamArray>] rest) =
        Seq.append [ head ] rest |> Seq.toList |> Qualified
    static member private regEx = Regex("^(?:(?<ns>[A-Za-z][A-Za-z0-9_]*)[.])*(?<name>[A-Za-z][A-Za-z0-9_]*)$") 
    static member TryParse(str, [<Out>] id : QualifiedIdentifier byref) =
        id <- Unchecked.defaultof<QualifiedIdentifier>
        let parsed = QualifiedIdentifier.regEx.Match(str)
        if parsed.Success |> not then false
        else
            let ns = 
                parsed.Groups.["ns"].Captures.Cast<Capture>()
                    |> Seq.map (fun p -> p.Value |> Identifier.Identifier)
            let name = parsed.Groups.["name"].Value |> Identifier.Identifier
            id <- Seq.append ns [ name ] |> Seq.toList |> Qualified
            true
    static member Parse id =
        match QualifiedIdentifier.TryParse id with
        | false, _ -> sprintf "Invalid qualified identifier '%s'" id |> invalidArg "id"
        | _, v -> v
    member __.Count = let (Qualified a) = __ in a.Length
    member __.Name = let (Qualified a) = __ in a.[a.Length - 1]
    member __.Namespace = let (Qualified a) = __ in Seq.take (a.Length - 1) a |> Array.ofSeq 
    member __.Item(i) = let (Qualified a) = __ in a.[i]
        
module Identifier =
    let parse id = Identifier.Parse id 
    let tryParse id = 
        match Identifier.TryParse id with
        | false, _ -> None
        | _, v -> Some v
    let create ns name =
        List.append ns [name] |> Qualified
    let parseQualified id = QualifiedIdentifier.Parse id
    let tryParseQualified id = 
        match QualifiedIdentifier.TryParse id with
        | false, _ -> None
        | _, v -> Some v
    let count (qid : QualifiedIdentifier) = qid.Count
    let name (qid : QualifiedIdentifier) = qid.Name
    let nameSpace (qid : QualifiedIdentifier) =
        let ns = qid.Namespace 
        if ns.Length = 0 then None else ns |> List.ofSeq |> Qualified |> Some 
        
 

