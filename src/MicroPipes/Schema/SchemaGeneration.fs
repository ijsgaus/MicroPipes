namespace MicroPipes.Schema
open System
open System.Reflection
open NuGet.Versioning
open MicroPipes.Schema.Green
open MicroPipes
open FSharp.Reflection

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
        if Object.Equals(uType, typeof<uint8>) then mks Enum8
        else
        if Object.Equals(uType, typeof<int8>) then mks Enum8u
        else
        if Object.Equals(uType, typeof<uint16>) then mks Enum16
        else
        if Object.Equals(uType, typeof<int16>) then mks Enum16u
        else
        if Object.Equals(uType, typeof<int>) then mks Enum32
        else
        if Object.Equals(uType, typeof<uint32>) then mks Enum32u
        else
        if Object.Equals(uType, typeof<int64>) then mks Enum64
        else
        if Object.Equals(uType, typeof<uint64>) then mks Enum64u
        else invalidOp "Unknown enum base data type"

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
        let isAlreadyKnown () =
            acc 
                |> List.map (fun p -> p.Declaration.Type) 
                |> List.filter Option.isSome
                |> List.map (fun p -> p.Value)
                |> List.contains typ
        
        let makeSchema t =
            { TypeSchema.Name = getTypeSchemaName typ; Declaration = { Body = t; Type = Some typ; Summary = None } }

        if wellknown |> List.contains typ || isAlreadyKnown() then acc
        else
            if typ.IsEnum then
                 (makeEnumSchema version typ |> TypeDefinition.EnumType |> makeSchema) :: acc
            else
                invalidOp "`12345"
                //if FSharpType.IsUnion(typ) then

                    

            

                
                


        

    //let generateFromTypes endpoints wellknown version schemaName = ()
           
