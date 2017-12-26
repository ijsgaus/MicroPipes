namespace MicroPipes.Schema
open System
open System.Reflection
open NuGet.Versioning
open MicroPipes.Schema.Green
open MicroPipes
open FSharp.Reflection
open TypePatterns

module SchemaGenerator =
    let isInVersion (version : SemanticVersion) (memb: MemberInfo) =
        match memb.GetCustomAttribute<VersionAttribute>() with
        | null -> true
        | ver ->
            let version = SemanticVersion(version.Major, version.Minor, version.Patch)
            if version < ver.FromVersion then false
            else
                if isNull ver.ToVersion then true
                else ver.ToVersion > version 

    let makeEnumGenericSchema version (typ : Type) (fn : EnumType<'t> -> EnumType) =
        let isFlag = typ.GetCustomAttribute<FlagsAttribute>() |> isNull |> not
        let fieldSchema (fld: FieldInfo) =
            { EnumField.Name = Identifier.parse(fld.Name); Value = fld.GetValue(null) |> unbox<'t>; Summary = None } 
        Enum.GetNames(typ) 
            |> Seq.map (fun p -> typ.GetField(p)) 
            |> Seq.filter (isInVersion version)
            |> Seq.map fieldSchema
            |> Seq.toList
            |> (fun p -> { EnumType.IsFlag = isFlag; Values = p }) |> fn

    let makeEnumSchema version typ =
        let mks a = makeEnumGenericSchema version typ a
        let uType = Enum.GetUnderlyingType(typ)
        match uType with
        | IsI8 -> mks Enum8
        | IsU8 ->  mks Enum8u
        | IsI16 -> mks Enum16
        | IsU16 -> mks Enum16u
        | IsI32 -> mks Enum32
        | IsU32 -> mks Enum32u
        | IsI64 -> mks Enum64
        | IsU64 -> mks Enum64u
        | _ -> invalidOp "Unknown enum base data type"

    let getTypeSchemaName (typ : Type) =
        match typ.GetCustomAttribute<NameInSchemaAttribute>() with
        | null -> typ.FullName |> Identifier.parseQualified
        | v -> v.Name |> Identifier.parseQualified

    

    let getOneOfTypeVariants (typ : Type) =
        if typ.IsAbstract |> not || typ.GetCustomAttribute<OneOfRootAttribute>() |> isNull then []
        else
            let getUnionName (t : Type) =
                match t.GetCustomAttribute<OneOfNameAttribute>() with
                | null -> t.Name.Replace("Type", "")
                | v -> v.Name
            typ.GetNestedTypes()
            |> Seq.filter (fun p -> typ.IsAssignableFrom(p))
            |> Seq.map (fun p -> (getUnionName p |> Identifier.parse , p))
            |> Seq.toList


    //let makeUnionSchema     

    let rec generateType wellknown (version : SemanticVersion) (acc : TypeSchema list) (typ : Type) = 
        let knownTypes list =
            list 
                |> List.map (fun p -> p.Declaration.Type, p) 
                |> List.filter (fun (p, _) -> p |> Option.isSome)
                |> List.map (fun (p, a) -> (p.Value, Reference { Name = a.Name; Type = a.Declaration.Type }))
        
        let makeSchema t =
            { TypeSchema.Name = getTypeSchemaName typ; Declaration = { Body = t; Type = Some typ; Summary = None } }

        let wk = knownTypes wellknown
        let ak = knownTypes acc

        match typ with
        | IsU8 -> U8 |> Ordinal |> Basic, acc
        | IsI8 -> I8 |> Ordinal |> Basic, acc
        | IsU16 -> U16 |> Ordinal |> Basic, acc
        | IsI16 -> U16 |> Ordinal |> Basic, acc
        | IsU32 -> U32 |> Ordinal |> Basic, acc
        | IsI32 -> I32 |> Ordinal |> Basic, acc
        | IsU64 -> U64 |> Ordinal |> Basic, acc
        | IsI64 -> I64 |> Ordinal |> Basic, acc
        | IsF32 -> F32 |> Float |> Basic, acc
        | IsF64 -> F64 |> Float |> Basic, acc
        | IsString -> BasicType.String |> Basic, acc
        | IsUuid -> Uuid |> Basic, acc
        | IsDT -> DT |> Basic, acc
        | IsDTO -> DTO |> Basic, acc
        | IsTS -> TS |> Basic, acc
        | IsBool -> BasicType.Bool |> Basic, acc
        | IsUrl -> Url |> Basic, acc
        | IsInList wk fst tr -> (snd tr, acc)
        | IsInList ak fst tr -> (snd tr, acc)
        | IsEnum -> 
                let es = makeEnumSchema version typ |> TypeDefinition.EnumType |> makeSchema
                Reference { Name = es.Name; Type = Some typ }, es :: acc
        | _ ->  invalidOp "`12345"
                //if FSharpType.IsUnion(typ) then

                    

            

                
                


        

    //let generateFromTypes endpoints wellknown version schemaName = ()
           
