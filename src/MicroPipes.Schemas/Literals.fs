module MicroPipes.Schemas.Literals
open System
open System.Text
open System.Runtime.InteropServices
open System.Text.RegularExpressions
open FParsec
open FSharp.LanguageExt


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

    
    
let pqualified<'t> : Parser<QualifiedIdentifier, 't> =
    let rec toQId lst =
        match lst with
        | [] -> invalidOp "Empty string mistake" // this cannot be by parser
        | [p] -> Simple p
        | h :: t -> Complex (h, toQId t)   
    sepBy1 pidentifier (pchar '.') |>> toQId    
    
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

type MapLiteral =
    | Named of HashMap<IdentifierIgnoreCaseEq, Identifier, Literal>
    | Indexed of Map<int, Literal>
    | Botch of HashMap<NameOrIndexEq, NameAndIndex, Literal> 
and Literal =
    | Identifier of QualifiedIdentifier
    | Basic of BasicLiteral 
    | Array of Literal list
    | Map of MapLiteral


