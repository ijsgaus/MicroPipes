namespace MicroPipes.Schema
open MicroPipes.Schema
open MicroPipes.Schema.Green
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open LanguageExt

type ValidationErrorSeverity =
    | Info = 1
    | Warning = 2
    | Error = 3

type ValidationError = ValidationErrorSeverity * string 

type ValidationResult = Result<unit, ValidationError list>

module Validation =
    let private checkDuplicates sel mk ss =
        ss
        |> List.groupBy sel
        |> List.filter (fun (_, v) -> v.Length > 1)
        |> List.map mk
    let private toErrors ss : ValidationError list =
        ss |> List.map (fun s -> ValidationErrorSeverity.Error, s)

    let private toResult ss : ValidationResult = 
        match ss with
        | [] -> Ok()
        | _ -> ss |> Error
    
    let validateEnum (enm : EnumType<'t>)  =
        let mkError (k, _) = sprintf "Duplicate enum field name '%O'" k
        enm.Values |> checkDuplicates (fun p -> p.Name) mkError 
            

    let validateEnumType enm =
        match enm with
        | Enum8u b -> validateEnum b
        | Enum8 sb -> validateEnum sb
        | Enum16u us -> validateEnum us
        | Enum16 s -> validateEnum s
        | Enum32u us -> validateEnum us
        | Enum32 s -> validateEnum s
        | Enum64u us -> validateEnum us
        | Enum64 s -> validateEnum s

    let validateNamedEntryList (nel : NamedEntry list) =
        let checkNames () =
            let mkError (key, _) = sprintf "Duplicate member name '%O'" key
            nel |> checkDuplicates (fun p -> p.Name) mkError
        let checkIndexes () =
            if nel |> List.exists (fun p -> p.Index.IsSome) then
                //existence
                let notExists =
                    nel 
                    |> List.filter (fun p -> p.Index.IsNone)
                    |> List.map (fun p -> sprintf "Index not specified for '%O'" p)
                // duplicates 
                let onDuplicate (k : int option, v : NamedEntry list) =    
                    let v = v |> List.map (fun p -> p.Name.ToString())
                    sprintf "Duplicate indexes %i on member %A" k.Value v
                let duplicates = 
                    nel 
                    |> List.filter (fun p -> p.Index.IsSome)       
                    |> checkDuplicates (fun p -> p.Index) onDuplicate
                duplicates |> List.append notExists
            else []
        List.append (checkNames()) (checkIndexes())
            
        

    let validateArgumentList (args : Argument list) =
        let mkError (key, _) = sprintf "Duplicate argument name '%O'" key
        args |> checkDuplicates (fun p -> p.Name) mkError 

    let validateEntryDuplicates name (ss : NamedSchemaEntry<_> list) =
        let mkError (key, _) = sprintf "Duplicate '%s' name '%O'" name key
        ss |> checkDuplicates (fun p -> p.Name) mkError 

    let validateEndpointsDuplicates name (ss : EndpointSchema list) =
        let mkError (key, _) = sprintf "Duplicate '%s' name '%O'" name key
        ss |> checkDuplicates (fun p -> p.Name) mkError 

    let validateType (t : NamedSchemaEntry<TypeDeclaration>)=
        (match t.Defintion.Body with
        | EnumType et -> validateEnumType et 
        | MapType mt -> validateNamedEntryList mt
        | OneOfType ot -> validateNamedEntryList ot)
        |> List.map (fun s -> sprintf "Type '%O': %s" t.Name s)

    let validateEndpoint (t : EndpointSchema) =
        (match t.Defintion with
        | Event _ -> []
        | Call cd -> cd.Arguments |> validateArgumentList)
        |> List.map (fun s -> sprintf "Endpoint '%O': %s" t.Name s)

    let validateSchema schema = 
        [
            schema.Types |> validateEntryDuplicates "type"
            schema.Types |> List.collect validateType
            schema.Endpoints |> validateEndpointsDuplicates "endpoint"
            schema.Endpoints |> List.collect validateEndpoint
        ] |> List.concat |> toErrors |> toResult

[<Extension>]
type ValidateExtensions private () =
    [<Extension>]
    static member Validate(schema, [<Out>] errors : byref<_>) =
        errors <- [] :> seq<_>
        match Validation.validateSchema schema with
        | Ok _ -> true
        | Error er -> errors <- er |> List.map snd; false
            


                    
                    

    



