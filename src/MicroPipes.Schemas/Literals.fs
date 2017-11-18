module HeavenPipe.Schemas.Literals
open System
open System.Text
open System.Runtime.InteropServices
open System.Text.RegularExpressions
open FParsec

 
   
        

type Identifier = 
        private | Identifier of string
        override __.ToString () = let (Identifier s) = __ in s
        
let pidentifier<'t> : Parser<Identifier, 't> =
       let isAsciiIdContinue = fun c -> isAsciiLetter c || isDigit c || c = '_'
       identifier (IdentifierOptions(
                           isAsciiIdStart = isAsciiLetter,
                           isAsciiIdContinue = isAsciiIdContinue
                           )) |>> Identifier
type Identifier with                           
    static member TryCreate (str) =
        match run pidentifier str with
        | Success(id, _, _) ->  id |> Result.Ok
        | Failure(msg, _, _) -> msg |> Result.Error
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

let pqualified<'t> : Parser<QualifiedIdentifier, 't> =
    let rec toQId lst =
        match lst with
        | [] -> invalidOp "Empty string mistake" // this cannot be by parser
        | [p] -> Simple p
        | h :: t -> Complex (h, toQId t)   
    sepBy1 pidentifier (pchar '.') |>> toQId    
    
type QualifiedIdentifier with    
    static member TryCreate qi =
        match run pqualified qi with
        | Success(id, _, _) ->  id |> Result.Ok
        | Failure(msg, _, _) -> msg |> Result.Error 
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


