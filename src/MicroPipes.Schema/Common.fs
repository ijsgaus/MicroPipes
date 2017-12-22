namespace MicroPipes.Schema

open System
open System.Linq
open System.Runtime.InteropServices
open System.Text
open System.Text.RegularExpressions
open Aliases



type Identifier = 
        private | Identifier of string
        override __.ToString () = let (Identifier s) = __ in s

module Identifier = 
    let private idRegEx = Regex("^[A-Za-z][A-Za-z0-9_]*$")
    
    let pidentifier s =
        let m = idRegEx.Match s
        match m.Success with
        | false -> None
        | true -> Identifier s |> Some
         
type Identifier with                           
    static member TryCreate (str) =
        match Identifier.pidentifier str with
        | Some id ->  id |> Result.Ok
        | None -> "Invalid identifier" |> Result.Error
    static member TryMake (str, [<Out>] id : Identifier byref)  = 
        match Identifier.TryCreate(str) with
        | Result.Ok s -> id <- s; true
        | _ -> id <- Unchecked.defaultof<Identifier>; false
    static member Create s =
        match Identifier.TryCreate s with
        | Result.Ok s -> s
        | Result.Error e -> raise (ArgumentException e)
        
    
type QualifiedIdentifier =
    private
    | Simple of Identifier
    | Complex of Identifier * QualifiedIdentifier
    override __.ToString() =
        let rec toStr qi =
            match qi with
            | Simple id -> id.ToString()
            | Complex (id, qid) -> id.ToString() + "." + (toStr qid)
        toStr __ 

module QualifiedIdentifier =
    /// <summary>
    /// First identifier in qualified identifier
    /// </summary>
    let head qid = match qid with | Simple id | Complex (id, _) -> id

    /// <summary>
    /// Qualified identifier without first identifier
    /// </summary>    
    let rest qid = 
        match qid with
        | Simple _ -> None
        | Complex (_, i) -> i |> Some
        
    /// <summary>
    /// Last identifier in quailifier identifier
    /// </summary>    
    let rec tail qid =
        match qid with
        | Simple t -> t
        | Complex (_, i) -> tail i
    
    /// <summary>
    /// Qualified identifier without last identifier
    /// </summary> 
    let rec start qid =
        match qid with
        | Simple _ -> None
        | Complex (t , r) ->
            match start r with
            | None -> Simple t |> Some
            | Some ci -> Complex(t, ci) |> Some

    /// <summary>
    /// Identifier count
    /// </summary> 
    let count qid = 
        let rec cnt id acc =
            match id with
            | Simple _ -> acc + 1 
            | Complex(_, r) -> cnt r (acc + 1)
        cnt qid 0
    let isAlone qid = match qid with | Simple _ -> true | _ -> false

    let private qidRegEx = Regex("^(?:(?<ns>[A-Za-z][A-Za-z0-9_]*)[.])*(?<name>[A-Za-z][A-Za-z0-9_]*)$")
    
    let pqualified s =
        let rec toQId lst =
            match lst with
            | [] -> invalidOp "Empty string mistake" // this cannot be by parser
            | [p] -> Simple p
            | h :: t -> Complex (h, toQId t)   
        let m = qidRegEx.Match(s)
        match m.Success with
        | false -> None
        | true ->
            let ns = m.Groups.["ns"]
            let nm = m.Groups.["name"].Captures.[0].Value
            match ns.Success with
            | false -> Identifier nm |> Simple |> Some
            | true ->   
                ns.Captures.Cast<Capture>() 
                    |> Seq.map (fun c -> c.Value) 
                    |> Seq.append [nm]
                    |> Seq.map Identifier
                    |> Seq.toList
                    |> toQId
                    |> Some    
    
type QualifiedIdentifier with 
    /// <summary>
    /// First identifier in qualified identifier
    /// </summary>   
    member __.Head () = QualifiedIdentifier.head __
    /// <summary>
    /// Qualified identifier without first identifier
    /// </summary>
    member __.TryGetRest([<Out>] id: _ byref) =
        match QualifiedIdentifier.rest __ with
        | Some p -> id <- p; true
        | None -> id <- Unchecked.defaultof<QualifiedIdentifier>; false
    /// <summary>
    /// Last identifier in quailifier identifier
    /// </summary>  
    member __.Rest() = QualifiedIdentifier.rest __
    /// <summary>
    /// Qualified identifier without last identifier
    /// </summary> 
    member __.Start([<Out>] id: _ byref) =
        match QualifiedIdentifier.start __ with
        | Some p -> id <- p; true
        | None -> id <- Unchecked.defaultof<QualifiedIdentifier>; false
    member __.Count = QualifiedIdentifier.count __
    member __.IsAlone = QualifiedIdentifier.isAlone __
    static member TryCreate qi =
        match QualifiedIdentifier.pqualified qi with
        | Some id ->  id |> Result.Ok
        | _ -> "Invalid qualified identifier" |> Result.Error 
    static member TryMake (str, [<Out>] id : QualifiedIdentifier byref)  = 
        match QualifiedIdentifier.TryCreate(str) with
        | Result.Ok s -> id <- s; true
        | _ -> id <- Unchecked.defaultof<QualifiedIdentifier>; false
    static member Create s =
        match QualifiedIdentifier.TryCreate s with
        | Result.Ok s -> s
        | Result.Error e -> raise (ArgumentException e)
   
 
type SemanticVersion = 
    {
        Major : uint16
        Minor : uint16
        Build : uint32
        Prerelease : string option
        PrereleaseNum : uint16 option
    }
    static member private RegEx = Regex("^(?<major>[0-9]+).(?<minor>[0-9]*).(?<build>[0-9]*)(?:(?:\-(?<pre>[a-z]+)(?<num>[0-9]+)?))?$")
    static member TryParse(s, [<Out>] ver : _ byref) =
        let p = SemanticVersion.RegEx.Match(s)
        ver <- { Major = 0us; Minor = 0us; Build = 0u; Prerelease = None; PrereleaseNum = None}
        match p.Success with
        | false -> false
        | _ ->
            let mjrs = p.Groups.["major"].Value
            let mnrs = p.Groups.["minor"].Value
            let blds = p.Groups.["build"].Value
            let pre = p.Groups.["pre"].Value
            let pres = p.Groups.["num"].Value
            match UInt16.TryParse(mjrs) with
            | false, _ -> false
            | _, mjr ->
                match UInt16.TryParse(mnrs) with
                | false, _ -> false
                | _, mnr ->
                    match UInt32.TryParse(blds) with
                    | false, _ -> false
                    | _, bld ->
                        match pre with
                        | "" ->  
                            ver <- { Major = mjr; Minor = mnr; Build = bld; Prerelease = None; PrereleaseNum = None}
                            true
                        | _ ->
                            match pres with
                            | "" -> 
                                ver <- { Major = mjr; Minor = mnr; Build = bld; Prerelease = Some pre; PrereleaseNum = None}
                                true
                            match UInt16.TryParse(pres) with
                            | false, _ -> false
                            | _, num ->
                                ver <- { Major = mjr; Minor = mnr; Build = bld; Prerelease = Some pre; PrereleaseNum = Some num}
                                true
    static member Parse s =
        match SemanticVersion.TryParse s with
        | false, _ -> invalidArg "s" "Invalid version string"
        | _, v -> v          
    override __.ToString() = 
        let bld = StringBuilder()
        bld.AppendFormat("{0}.{1}.{2}", __.Major, __.Minor, __.Build) |> ignore
        match __.Prerelease with
        | Some s -> 
            bld.AppendFormat("-{0}", s) |> ignore
            match __.PrereleaseNum with
            | Some i -> bld.Append(i) |> ignore
            | None -> ()
        | None -> ()
        bld.ToString()
          
[<Struct>]    
type IdentifierIgnoreCaseEq =
    interface Eq<Identifier> with
        member __.Equals (x , y) =
            StringComparer.InvariantCultureIgnoreCase.Equals(x.ToString(), y.ToString())
        member __.GetHashCode x =
            StringComparer.InvariantCultureIgnoreCase.GetHashCode (x.ToString())                

type NameAndIndex =
    {
        Name : Identifier
        Index : int
    }

[<Struct>]
type NameOrIndexEq =
    interface Eq<NameAndIndex> with
        member __.Equals (x, y) =
            StringComparer.InvariantCultureIgnoreCase.Equals(x.Name.ToString(), y.Name.ToString()) || x.Index = y.Index
        member __.GetHashCode x =
            x.Index.GetHashCode()