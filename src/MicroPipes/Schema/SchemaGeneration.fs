namespace MicroPipes.Schema
open System
open System.Reflection
open NuGet.Versioning
open MicroPipes.Schema.Green
open MicroPipes
open System.Linq
open TypePatterns
open System

module SchemaGenerator =
    type TypeRecognizer<'t> = Type -> Option<'t>
    type DocReader = MemberInfo -> string option
    type CheckVersion = MemberInfo -> bool
    type EnumMaker = CheckVersion -> DocReader -> Type -> Option<EnumType>
    
    let (>=>) a b =
        fun x -> 
            match a x with
            | Some y -> b y
            | None -> None

    let (>->) a b =
        fun x -> 
            match a x with
            | Some y -> b y |> Some
            | None -> None
    
    let (=|>) a b =
        fun x ->
            match a x with
            | Some y -> Some y
            | _ -> b x
    
    let (?=) a b =
        fun x ->
            match a x with
            | Some y -> y
            | _ -> b x

    let getAttribute<'t when 't :> Attribute and 't: null> (mi : MemberInfo) =
        match mi.GetCustomAttribute<'t>() with
        | null -> None
        | a -> Some a

    let isInVersion (version : SemanticVersion) =
        getAttribute<VersionRangeAttribute> 
            >=> (fun x -> x.IsInRange(version) |> Some) 
            ?= (fun _ -> true)

    let memberName =
        getAttribute<MemberIdentifierAttribute> >=> (fun o -> o.Name |> Some) ?= (fun (mi : MemberInfo) -> Identifier.parse mi.Name)

    let typeName =
        getAttribute<SchemaIdentifierAttribute> >=> (fun o -> o.Name |> Some) ?= (fun (t : Type) -> Identifier.parseQualified (t.Name.Replace("+", ".")))

    let makeEnumFromType<'t> (checkVersion : CheckVersion) (docReader : DocReader) =
        fun (t : Type) ->
            if t.IsEnum then 
                let isFlag = 
                    getAttribute<FlagsAttribute> t 
                    |> Option.map (fun _ -> true) 
                    |> Option.defaultValue false
                let field (fi : FieldInfo) =
                   { EnumField.Name = memberName fi; Value = fi.GetValue(null) |> unbox<'t>; Summary = docReader fi }
                Enum.GetNames(t) 
                |> Seq.map (fun p -> t.GetField(p)) 
                |> Seq.filter checkVersion
                |> Seq.map field
                |> Seq.toList 
                |> (fun p -> { EnumType.IsFlag = isFlag; Values = p }) 
                |> Some
            else None    
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
    
    let isType<'t> (t:Type) = if Object.Equals(typeof<'t>, t) then Some(t) else None
    let isEnumBaseType<'t> (t : Type) =
        match Enum.GetUnderlyingType(t) with
        | null -> None
        | p -> isType<'t> p |> Option.map (fun _ -> t)

    type Configuration =
        {
            IsInVersion : CheckVersion
            DocReader : DocReader
            EnumMakers : EnumMaker list
        }
        static member Default version =
            {
                IsInVersion = isInVersion version
                DocReader = fun _ -> None
                EnumMakers = 
                    [  
                        fun cv dr  -> isEnumBaseType<byte>   >=> makeEnumFromType cv dr >-> Enum8u
                        fun cv dr  -> isEnumBaseType<sbyte>  >=> makeEnumFromType cv dr >-> Enum8
                        fun cv dr  -> isEnumBaseType<uint16> >=> makeEnumFromType cv dr >-> Enum16u
                        fun cv dr  -> isEnumBaseType<int16>  >=> makeEnumFromType cv dr >-> Enum16
                        fun cv dr  -> isEnumBaseType<uint32> >=> makeEnumFromType cv dr >-> Enum32u
                        fun cv dr  -> isEnumBaseType<int>    >=> makeEnumFromType cv dr >-> Enum32
                        fun cv dr  -> isEnumBaseType<uint64> >=> makeEnumFromType cv dr >-> Enum64u
                        fun cv dr  -> isEnumBaseType<int64>  >=> makeEnumFromType cv dr >-> Enum64
                    ]

            }

    
            

    let makeEnumSchema config typ =
        config.EnumMakers 
            |> Seq.map (fun p -> p config.IsInVersion config.DocReader typ)
            |> Seq.tryFind Option.isSome
            |> Option.bind id
            |> Option.defaultWith (fun () -> invalidOp "Unknown enum base data type")
        (*
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
        | _ -> invalidOp "Unknown enum base data type"*)

    let getTypeSchemaName (typ : Type) =
        match typ.GetCustomAttribute<SchemaIdentifierAttribute>() with
        | null -> typ.FullName.Replace("+", ".") |> Identifier.parseQualified
        | v -> v.Name 

    let getFieldName (mi: MemberInfo) =
        match mi.GetCustomAttribute<MemberIdentifierAttribute>() with
        | null -> mi.Name |> Identifier.parse
        | v -> v.Name

    let getOneOfTypeVariants (typ : Type) =
        if typ.IsAbstract |> not || typ.GetCustomAttribute<OneOfRootAttribute>() |> isNull then []
        else
            let getUnionName (t : Type) =
                match t.GetCustomAttribute<OneOfMemberAttribute>() with
                | null -> t.Name.Replace("Type", "") |> Identifier.parse
                | v -> v.Name
            typ.GetNestedTypes()
            |> Seq.filter (fun p -> typ.IsAssignableFrom(p))
            |> Seq.map (fun p -> (getUnionName p, p))
            |> Seq.toList

    let rec generateTypeByPropList (version : SemanticVersion) (acc : TypeSchema list) id (props : PropertyInfo list) typ  =
        let folder (ts, fl) (prop : PropertyInfo)  =
            let (r, ts1) = generateType version ts (prop.PropertyType)
            ts1, { NamedEntry.Name = getFieldName prop; Index = None; Type = r; Summary = None } :: fl
        let (ts, fl) = props |> List.fold folder (acc, [])
        let nt = { TypeSchema.Name = id; Declaration = { Body = MapType (fl |> List.rev); Type = typ; Summary = None } }
        let tr = Reference { Name = id; Type = typ }
        match ts |> List.tryFind (fun p -> p.Name = id && (match p.Declaration.Body with | Dummy -> true | _ -> false )) with
        | Some t ->  tr,  ts |> List.map (fun p -> if p = t then nt else p)
        | _ -> tr, nt:: ts
    and generateType (version : SemanticVersion) (acc : TypeSchema list) (typ : Type) = 
        let knownTypes list =
            list 
                |> List.map (fun p -> p.Declaration.Type, p) 
                |> List.filter (fun (p, _) -> p |> Option.isSome)
                |> List.map (fun (p, a) -> (p.Value, Reference { Name = a.Name; Type = a.Declaration.Type }))
        
        let makeSchema t =
            { TypeSchema.Name = getTypeSchemaName typ; Declaration = { Body = t; Type = Some typ; Summary = None } }

        let ak = knownTypes acc

        match typ with
        | IsU8 -> U8 |> Ordinal |> Basic, acc
        | IsI8 -> I8 |> Ordinal |> Basic, acc
        | IsU16 -> U16 |> Ordinal |> Basic, acc
        | IsI16 -> I16 |> Ordinal |> Basic, acc
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
        | IsUnit -> Unit, acc
        | IsInList ak fst tr -> (snd tr, acc)
        | IsOptionLike el ->
            let er, acc1 = generateType version acc el
            TypeReference.MayBe er, acc1
        | IsArray el
        | IsArrayLike el -> 
            let er, acc1 = generateType version acc el
            TypeReference.Array er, acc1
        | IsTuple el 
        | IsValueTuple el ->
            match el with
            | [] -> TypeReference.Unit, acc
            | _ ->
                let rec pelem v ts elr =
                    match v with
                    | [] -> elr, ts
                    | [h] -> 
                        let r, ts1 = generateType version ts h
                        r :: elr, ts1
                    | h :: rest -> 
                        let r, ts1 = generateType version ts h
                        pelem rest ts1 (r :: elr)
                let (elr, ts) = pelem el acc []
                TypeReference.Tuple (elr |> List.rev), ts
        | IsEnum -> 
                let es = makeEnumSchema (Configuration.Default version) typ |> TypeDefinition.EnumType |> makeSchema
                Reference { Name = es.Name; Type = Some typ }, es :: acc
        | IsUnion ul ->
            let uid = getTypeSchemaName typ
            let ru = Reference { Name = uid; Type = Some typ }
            let dummy = { TypeSchema.Name = uid; Declaration = { Body = Dummy; Type = Some typ ; Summary = None } }
            let es =  dummy :: acc
            let folder (ts, tr) (id, attrs, ind, props : PropertyInfo list)  =
                match attrs |> Enumerable.OfType<VersionRangeAttribute> |> Seq.tryHead with
                | Some t when t.IsInRange(version) |> not -> ts, tr
                | _ -> 
                    let flds = 
                        props
                        |> List.filter (isInVersion version)
                    match flds with
                    | [] -> ts, { NamedEntry.Name = id; Index = ind |> Some; Type = TypeReference.Unit; Summary = None } :: tr
                    | [h] -> 
                             let (r, ts1) = generateType version ts (h.PropertyType)
                             ts1, { NamedEntry.Name = id; Index = ind |> Some; Type = r; Summary = None } :: tr
                    | _ -> 
                        let (r, ts1) = generateTypeByPropList version ts (Identifier.create uid id) flds None
                        ts1, { NamedEntry.Name = id; Index = ind |> Some; Type = r; Summary = None } :: tr
            let (ts1, fls) = ul |> List.fold folder (es, [])
            ru, ts1 |> List.map (fun p -> if p = dummy then { Name = uid; Declaration = { Body = fls |> List.rev |> OneOfType; Type = Some typ; Summary = None }} else p)

        | _ ->  invalidOp "`12345"
                //if FSharpType.IsUnion(typ) then

                    

            

                
                


        

    //let generateFromTypes endpoints wellknown version schemaName = ()
           
