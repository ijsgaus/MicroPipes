namespace MicroPipes.Schema
open System
open System.Reflection
open NuGet.Versioning
open MicroPipes.Schema.Green
open MicroPipes
open System.Linq
open TypePatterns
open System
open MicroPipes.Reflection

type ISchemaGeneratorExtension =
    abstract ExtendEnum : enumType: Type * enumNode: EnumType -> EnumType
    abstract ExtendProperty : memb: MemberInfo * entry : NamedEntry -> NamedEntry 
    abstract ExtendUnionCase : case : MakedFrom * entry : NamedEntry -> NamedEntry
    abstract ExtendType : typ : MakedFrom * node: TypeDeclaration -> TypeDeclaration
    abstract ExtendEndpoint : typ : Type * schemaName : QualifiedIdentifier * endpoint : EndpointSchema -> EndpointSchema

module SchemaGenerator =
    
    let isInVersion (version : SemanticVersion) =
        fun t -> 
            match box t with
            | :? MemberInfo as mi -> 
                getAttribute<VersionRangeAttribute> mi 
                |> Option.map (fun x -> x.Satisfies(version)) 
                |> Option.defaultValue true
            | :? FSharp.Reflection.UnionCaseInfo as uci ->
                uci.GetCustomAttributes()
                |> Enumerable.OfType<VersionRangeAttribute>
                |> Seq.tryHead
                |> Option.map (fun x -> x.Satisfies(version)) 
                |> Option.defaultValue true
            | _ -> false
            

    let memberName =
        getAttribute<MemberIdentifierAttribute> >=> (fun o -> o.Name |> Some) ?= (fun (mi : MemberInfo) -> Identifier.parse mi.Name)

    let typeName =
        getAttribute<SchemaIdentifierAttribute> >=> (fun o -> o.Name |> Some) ?= (fun (t : Type) -> Identifier.parseQualified (t.FullName.Replace("+", ".")))

   

    type IServices =
        abstract IsInVersion<'t> : SemanticVersion -> 't -> bool
        abstract TypeToName : Type -> QualifiedIdentifier
        abstract MemberToName<'t when 't :> MemberInfo> : 't -> Identifier
        abstract DocReader<'t> : 't -> string option

     

    let makeGenericEnumType<'t> (services : IServices) version  =
        fun (t : Type) ->
            if t.IsEnum then 
                let isFlag = 
                    getAttribute<FlagsAttribute> t 
                    |> Option.map (fun _ -> true) 
                    |> Option.defaultValue false
                let field (fi : FieldInfo) =
                    { 
                       FieldName = services.MemberToName fi
                       Value = fi.GetValue(null) |> unbox<'t>
                       Summary = services.DocReader fi
                    }
                Enum.GetNames(t) 
                |> Seq.map (fun p -> t.GetField(p)) 
                |> Seq.filter (services.IsInVersion version)
                |> Seq.map field
                |> Seq.toList 
                |> (fun p -> { IsFlag = isFlag; Values = p; Extensions = Map.empty }) 
                |> Some
            else None    
    
    
    

    let makeEnumType<'t> (cvt : EnumType<'t> -> EnumType) =
        fun cv dr -> isEnumBaseType<'t> >=> makeGenericEnumType cv dr >-> cvt

    type EnumMaker = IServices -> SemanticVersion -> Type -> Option<EnumType>  



    

    type Configuration =
        {
            Services : IServices
            EnumMakers : EnumMaker list
            Extensions : ISchemaGeneratorExtension list
        }
        static member Default  =
            {
                Services = 
                    {
                        new IServices with    
                            member __.IsInVersion v m = isInVersion v m
                            member __.TypeToName t = typeName t
                            member __.MemberToName m = memberName m
                            member __.DocReader _ = None
                    }
                EnumMakers = 
                    [  
                        makeEnumType<byte>   Enum8u
                        makeEnumType<sbyte>  Enum8
                        makeEnumType<uint16> Enum16u
                        makeEnumType<int16>  Enum16
                        makeEnumType<uint32> Enum32u
                        makeEnumType<int>    Enum32
                        makeEnumType<uint64> Enum64u
                        makeEnumType<int64>  Enum64
                    ]
                Extensions = []
            }

    
            

    let makeEnumSchema config version  =
        fun typ ->
          let etype =
            config.EnumMakers 
            |> Seq.map (fun p -> p config.Services version typ)
            |> Seq.tryFind Option.isSome
            |> Option.bind id
            |> Option.defaultWith (fun () -> invalidOp "Unknown enum base data type")
          config.Extensions |> List.fold (fun etype ex -> ex.ExtendEnum(typ, etype)) etype
            
        


    let rec generateTypeByPropList config (version : SemanticVersion) (typeList : TypeDeclaration list) decl (props : PropertyInfo list)   =
        let makeField (typeList, fields) (prop : PropertyInfo)  =
            let (field, typeList) = generateType config version typeList (prop.PropertyType)
            let field = 
                { 
                    MemberName = config.Services.MemberToName prop 
                    Index = None 
                    TypeRef = field 
                    Summary = None
                    Extensions = Map.empty 
                }
            let field = config.Extensions |> List.fold (fun fld ex -> ex.ExtendProperty(prop, fld)) field
            typeList, field :: fields

        let (typeList, fields) = props |> List.fold makeField (typeList, [])
        let typeNode =
            { 
                TypeName = decl.TypeName 
                Body = MapType (fields |> List.rev)
                Type = decl.Type
                Summary = decl.Summary
                Extensions = decl.Extensions 
            }
        typeNode, typeList

    and generateType config (version : SemanticVersion) (typeList : TypeDeclaration list) (typ : Type) = 
        let toTypeMap (typeList : TypeDeclaration list) =
            typeList 
                |> Seq.map (fun p -> p.Type, p) 
                |> Seq.collect 
                        (fun (p, v) -> 
                            match p with
                            | None -> []
                            | Some a -> 
                                match a with
                                | RealType t -> [t, Reference { RefName = v.TypeName; Type = t |> Some }]
                                | _ -> [])
                |> HashMap.fromSeq
        
        let makeSchema t =
            { TypeName = config.Services.TypeToName typ; Body = t; Type = RealType typ |> Some; Summary = None; Extensions = Map.empty }

        let typeMap = toTypeMap typeList

        match typ with
        | IsU8 -> U8 |> Ordinal |> Basic, typeList
        | IsI8 -> I8 |> Ordinal |> Basic, typeList
        | IsU16 -> U16 |> Ordinal |> Basic, typeList
        | IsI16 -> I16 |> Ordinal |> Basic, typeList
        | IsU32 -> U32 |> Ordinal |> Basic, typeList
        | IsI32 -> I32 |> Ordinal |> Basic, typeList
        | IsU64 -> U64 |> Ordinal |> Basic, typeList
        | IsI64 -> I64 |> Ordinal |> Basic, typeList
        | IsF32 -> F32 |> Float |> Basic, typeList
        | IsF64 -> F64 |> Float |> Basic, typeList
        | IsString -> BasicType.String |> Basic, typeList
        | IsUuid -> Uuid |> Basic, typeList
        | IsDT -> DT |> Basic, typeList
        | IsDTO -> DTO |> Basic, typeList
        | IsTS -> TS |> Basic, typeList
        | IsBool -> BasicType.Bool |> Basic, typeList
        | IsUrl -> Url |> Basic, typeList
        | IsUnit -> Unit, typeList
        | IsKnown typeMap reference -> (reference, typeList)
        | IsOptionLike el ->
            let reference, typeList = generateType config version typeList el
            MayBe reference, typeList
        | IsArray el
        | IsArrayLike el -> 
            let reference, typeList = generateType config version typeList el
            TypeReference.Array reference, typeList
        | IsTuple els 
        | IsValueTuple els ->
            match els with
            | [] -> Unit, typeList
            | _ ->
                let pelem (refs, typeList) typ =
                    let ref, typeList = generateType config version typeList typ
                    ref :: refs, typeList 
                let (refs, typeList) = els |> List.fold pelem ([], typeList)
                TypeReference.Tuple (refs |> List.rev), typeList
        | IsEnum -> 
                let es = makeEnumSchema Configuration.Default version typ |> TypeDefinition.EnumType |> makeSchema
                Reference { RefName = es.TypeName; Type = Some typ }, es :: typeList
        | IsUnion ul ->
            let unionId = config.Services.TypeToName typ
            let unionRef = Reference { RefName = unionId; Type = Some typ }
            let unionDummy = 
                { 
                    TypeName = unionId 
                    Body = Dummy 
                    Type = Some(RealType typ)  
                    Summary = config.Services.DocReader typ 
                    Extensions = Map.empty 
                }
            let typeList =  unionDummy :: typeList
            let caseFolder (typeList, cases) (caseInfo : FSharp.Reflection.UnionCaseInfo)  =
                if config.Services.IsInVersion version caseInfo |> not then typeList, cases
                else
                    let caseId = caseInfo.Name |> Identifier.parse
                    let flds = 
                        caseInfo.GetFields()
                        |> Seq.filter (config.Services.IsInVersion version)
                        |> Seq.toList
                    match flds with
                    | [] -> typeList, 
                                { 
                                    MemberName = caseId 
                                    Index = None 
                                    TypeRef = TypeReference.Unit
                                    Summary = config.Services.DocReader caseInfo
                                    Extensions = Map.empty 
                                } :: cases
                    | [h] -> 
                             let (r, typeList) = generateType config version typeList (h.PropertyType)
                             typeList, 
                                { 
                                    MemberName = caseId 
                                    Index = None 
                                    TypeRef = r 
                                    Summary = config.Services.DocReader caseInfo 
                                    Extensions = Map.empty
                                } :: cases
                    | _ -> 
                        let dummyMember = 
                            { 
                                TypeName = Identifier.create unionId caseId
                                Body = Dummy
                                Type = Some(UnionCase caseInfo)  
                                Summary = config.Services.DocReader caseInfo  
                                Extensions = Map.empty 
                            }
                        let (r, typeList) = generateTypeByPropList config version typeList dummyMember flds 
                        let r = config.Extensions |> List.fold (fun st p -> p.ExtendType(UnionCase caseInfo, st)) r
                        let memb = 
                            { 
                                MemberName = caseId 
                                Index = None 
                                TypeRef = Reference { RefName = dummyMember.TypeName; Type = None } 
                                Summary = config.Services.DocReader caseInfo 
                                Extensions = Map.empty
                            }
                        let memb = config.Extensions |> List.fold (fun st p -> p.ExtendUnionCase((UnionCase caseInfo), st)) memb
                        typeList |> List.map (fun p -> if p = dummyMember then r else p), memb :: cases
            let (typeList, cases) = ul |> List.fold caseFolder (typeList, [])
            let unionNode =
                { 
                    TypeName = unionDummy.TypeName 
                    Body = cases |> List.rev |> OneOfType 
                    Type = unionDummy.Type 
                    Summary = unionDummy.Summary 
                    Extensions = Map.empty 
                }
            let unionNode = config.Extensions |> List.fold (fun st p -> p.ExtendType((RealType typ), st)) unionNode 
            unionRef, 
                typeList 
                |> List.map (fun p -> if p = unionDummy then unionNode else p)

        | _ ->  invalidOp "`12345"
                //if FSharpType.IsUnion(typ) then

                    

            

                
                


        

    //let generateFromTypes endpoints wellknown version schemaName = ()
           
