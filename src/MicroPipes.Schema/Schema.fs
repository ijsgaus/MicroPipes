namespace rec MicroPipes.Schema
open System
open Aliases
open MicroPipes.Schema.Green
open System.Collections.Immutable

exception ServiceSchemaException of string

type ServiceSchema(schema) =
    new (name, version) =
        let qname = QualifiedIdentifier.Create name
        let ver = SemanticVersion.Parse version
        let sch = { Name = qname; Version = ver; Types = ImmutableDictionary<_, _>.Empty; Endpoints = ImmutableDictionary<_, _>.Empty } 
        ServiceSchema(sch)
    member __.Green = schema
    
module ServiceSchema =
    let typeByReference (sch : ServiceSchema) r =
        match r with
        | UnitType -> Unit
        | BasicType bt -> TypeSchema.Basic bt
        | ArrayType _ -> ArraySchema(sch, r) |> TypeSchema.Array
        | MayBeType _ -> MayBeSchema(sch, r) |> TypeSchema.MayBe
        | TupleType _ -> TupleSchema(sch, r) |> TypeSchema.Tuple
        | NamedType nt ->
            match sch.Green.Types.TryGetValue(nt) with
            | false, _ -> sprintf "Unknown type name %s" (nt.ToString()) |> ServiceSchemaException |> raise
            | _, fnd ->
                match fnd.Body with
                | TypeDeclarationGreen.WellKnown -> TypeSchema.WellKnown(nt, fnd.Summary)
                | TypeDeclarationGreen.EnumType et -> TypeSchema.Enum(nt, et, fnd.Summary)
                 
                
        
type ArraySchema internal (root , green) =
    let get =
        match green with
        | ArrayType gt -> gt
        | _ -> ServiceSchemaException "Invalid type of green node" |> raise
    let et = lazy (ServiceSchema.typeByReference root get)
    member __.Green = green
    member __.ElementType = et.Force()
    
type MayBeSchema internal (root, green) =
    let get =
        match green with
        | MayBeType gt -> gt
        | _ -> ServiceSchemaException "Invalid type of green node" |> raise
    let et = lazy (ServiceSchema.typeByReference root get)
    member __.Green = green
    member __.ElementType = et.Force()
    
type TupleSchema internal (root, green) =
    let get =
        match green with
        | TupleType gt -> gt
        | _ -> ServiceSchemaException "Invalid type of green node" |> raise
    let et = lazy (get |> List.map (ServiceSchema.typeByReference root) )
    member __.Green = green
    member __.ElementTypes = et.Force()
    

type NamedDesc =
    {
        Name : Identifier
        Index : int option
        Summary : string option
        Type : TypeSchema
    }    
    
type MapSchema internal (root, green) =
    let get =
        match green.Body with
        | MapType gt -> gt
        | _ -> ServiceSchemaException "Invalid type of green node" |> raise
    
    

type TypeSchema =
    | Unit
    | WellKnown of QualifiedIdentifier * (string option)
    | Basic of BasicType
    | Array of ArraySchema
    | MayBe of MayBeSchema
    | Tuple of TupleSchema
    | Enum of Name: QualifiedIdentifier * Body: EnumTypeDescription * Summary: string option
    
    

    