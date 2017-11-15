module HeavenPipe.Schemas.Literals
open System
open System.Runtime.InteropServices
open System.Text.RegularExpressions

let private identifierRegEx = Regex("""^[^\d\W]\w*\Z""")

type Identifier = 
        private | Identifier of string 
        static member TryCreate (str) =
            if identifierRegEx.IsMatch(str) then Identifier str |> Ok else Error "Invalid identifier"
        static member TryMake (str, [<Out>] id : Identifier byref)  = 
            match Identifier.TryCreate(str) with
            | Ok s -> id <- s; true
            | _ -> id <- Unchecked.defaultof<Identifier>; false
        static member Create s =
            match Identifier.TryCreate s with
            | Ok s -> s
            | Error e -> raise (ArgumentException e)
    
type QualifiedIdentifier =
    | Simple of Identifier
    | Complex of Identifier * QualifiedIdentifier
    static member TryCreate (ns: string) =
        let rec fromList lst =
            match lst with
            | [] -> Error("empty name")
            | [id] -> Simple id |> Ok
            | id :: tail -> fromList tail |> Result.map (fun p -> Complex(id, p))
        let folder state el =     
            match state with
            | Ok p ->
                match el with
                | Ok e -> Ok(e :: p)
                | Error s -> Error s
            | Error s ->
                match el with
                | Error s1 -> Error (s1 + Environment.NewLine + s)
                | _ -> state
                    
        let splitted = 
            ns.Split(':') 
                |> Array.toList 
                |> List.map Identifier.TryCreate
                |> List.rev
                |> List.fold folder (Ok []) 
        match splitted with
        | Error s -> Error s
        | Ok sp -> fromList sp 
    static member TryMake (str, [<Out>] id : QualifiedIdentifier byref) =
        match QualifiedIdentifier.TryCreate(str) with
        | Ok s -> id <- s; true
        | _ -> id <- Unchecked.defaultof<QualifiedIdentifier>; false
    static member Create s =
        match QualifiedIdentifier.TryCreate s with
        | Ok s -> s
        | Error e -> raise (ArgumentException e)
        

type OrdinalLiteral =
    | U8Literal of byte
    | I8Literal of sbyte
    | U16Literal of uint16
    | I16Literal of int16
    | U32Literal of uint32
    | I32Literal of int32
    | U64Literal of uint64
    | I64Literal of int64


type BasicLiteral =
    | OrdinalLiteral of OrdinalLiteral
    | F32Literal of float32
    | F64Literal of float
    | StringLiteral of string
    | UuidLiteral of Guid
    | BoolLiteral of bool
    | DTLiteral of DateTime
    | DTOLiteral of DateTimeOffset
    | TSLiteral of TimeSpan
    | NoneLiteral

[<CustomEquality>]
[<CustomComparison>]
type UrlLiteral = 
    | UrlLiteral of Uri
    override __.Equals(other) =
            match other with
            | :? UrlLiteral as o ->  
                let (UrlLiteral x) = __
                let (UrlLiteral y) = o
                x.ToString() = y.ToString()
            | _ -> false
    override __.GetHashCode() = let (UrlLiteral a) = __ in a.GetHashCode()
    interface IEquatable<UrlLiteral> with
        member __.Equals other =
            let (UrlLiteral x) = __
            let (UrlLiteral y) = other
            x.ToString() = y.ToString()
    interface IComparable<UrlLiteral> with
        member __.CompareTo other =
            let (UrlLiteral x) = __
            let (UrlLiteral y) = other
            StringComparer.InvariantCulture.Compare(x.ToString(), y.ToString())
    interface IComparable with
        member __.CompareTo other =
            match other with
            | :? UrlLiteral as y -> (__ :> IComparable<UrlLiteral>).CompareTo(y)
            | _ -> invalidArg "other" "cannot compare values of different types"

type Literal =
    | Identifier of QualifiedIdentifier
    | Url of UrlLiteral
    | Basic of BasicLiteral 
    | Array of Literal list
    | Map of Map<Identifier, Literal>


