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
open System.Security.Cryptography


type ISchemaGeneratorExtension =
    abstract ExtendEnum : enumType: Type * enumNode: EnumType -> EnumType
    abstract ExtendProperty : memb: MemberInfo * entry : NamedEntry -> NamedEntry 
    abstract ExtendUnionCase : case : Maked<Type> * entry : NamedEntry -> NamedEntry
    abstract ExtendType : typ : Maked<Type> * node: TypeDeclaration -> TypeDeclaration
    abstract ExtendEndpoint : typ : Type * schemaName : QualifiedIdentifier * endpoint : EndpointSchema -> EndpointSchema

module SchemaGenerator =
    
    let isInVersion (version : SemanticVersion) =
        fun t -> 
            match t with
            | Member mi -> 
                getAttribute<VersionRangeAttribute> mi 
                |> Option.map (fun x -> x.Satisfies(version)) 
                |> Option.defaultValue true
            | UnionCase uci ->
                uci.GetCustomAttributes()
                |> Enumerable.OfType<VersionRangeAttribute>
                |> Seq.tryHead
                |> Option.map (fun x -> x.Satisfies(version)) 
                |> Option.defaultValue true
            
            

    let memberName =
        fun (t : Maked<MemberInfo>) ->
            match t with
            | Member rt ->    
                rt  
                |> getAttribute<MemberIdentifierAttribute> 
                |> Option.map  (fun o -> o.Name)
                |> Option.defaultWith (fun () -> 
                                            Identifier.parse 
                                                    (match rt with 
                                                    | :? Type as t -> if t.Name.EndsWith("Type") then t.Name.Substring(0, t.Name.Length - 4) else t.Name 
                                                    | _ ->  rt.Name)) 
            | UnionCase uc ->
                uc.GetCustomAttributes()  
                |> Enumerable.OfType<MemberIdentifierAttribute>
                |> Seq.tryHead  
                |> Option.map  (fun o -> o.Name)
                |> Option.defaultWith (fun () -> Identifier.parse uc.Name) 
        

    let typeName =
        fun (t : Maked<Type>)  ->
            match t with
            | Member rt ->    
                    rt  |> getAttribute<SchemaIdentifierAttribute> 
                        |> Option.map  (fun o -> o.Name)
                        |> Option.defaultWith (fun () -> Identifier.parseQualified (rt.FullName.Replace("+", "."))) 
            | UnionCase uc ->
                    uc.GetCustomAttributes()  
                    |> Enumerable.OfType<SchemaIdentifierAttribute>
                    |> Seq.tryHead  
                    |> Option.map  (fun o -> o.Name)
                    |> Option.defaultWith (fun () -> Identifier.parseQualified (uc.DeclaringType.FullName.Replace("+", ".") + "." + uc.Name)) 
                                   

   

    type Services =
        {
            IsInVersion : SemanticVersion -> Maked<MemberInfo> -> bool
            TypeToName : Maked<Type> -> QualifiedIdentifier
            MemberToIndex : Maked<MemberInfo> -> int option
            MemberToName : Maked<MemberInfo> -> Identifier
            DocReader : Maked<MemberInfo> -> string option
        }

     

    let makeGenericEnumType<'t> (services : Services) version  =
        fun (t : Type) ->
            if t.IsEnum then 
                let isFlag = 
                    getAttribute<FlagsAttribute> t 
                    |> Option.map (fun _ -> true) 
                    |> Option.defaultValue false
                let field (fi : FieldInfo) =
                    { 
                       FieldName = Maked.fromMember fi |> services.MemberToName 
                       Value = fi.GetValue(null) |> unbox<'t>
                       Summary = Maked.fromMember fi |> services.DocReader
                    }
                Enum.GetNames(t) 
                |> Seq.map (fun p -> t.GetField(p)) 
                |> Seq.filter (Maked.fromMember >> services.IsInVersion version)
                |> Seq.map field
                |> Seq.toList 
                |> (fun p -> { IsFlag = isFlag; Values = p; Extensions = Map.empty }) 
                |> Some
            else None    
    
    
    

    let makeEnumType<'t> (cvt : EnumType<'t> -> EnumType) =
        fun cv dr -> isEnumBaseType<'t> >=> makeGenericEnumType cv dr >-> cvt

    type EnumMaker = Services -> SemanticVersion -> Type -> Option<EnumType>  



    

    type Configuration =
        {
            Services : Services
            EnumMakers : EnumMaker list
            Extensions : ISchemaGeneratorExtension list
        }
        static member Default  =
            {
                Services = 
                    {
                        IsInVersion = isInVersion
                        TypeToName = typeName 
                        MemberToName = memberName
                        MemberToIndex = (fun _ -> None)
                        DocReader = (fun _ -> None)
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
            
        

    let makeTypeSchema (config : Configuration) version =
        let extendType t ts = config.Extensions |> List.fold (fun st p -> p.ExtendType(t, st)) ts
        fun typeList (typ : Type) ->
            let rec makeTypeByProps typeList (props : PropertyInfo seq) =
                let makeField (typeList, fields) (prop : PropertyInfo)  =
                    let (field, typeList) = generateType typeList (prop.PropertyType)
                    let field = 
                        { 
                            MemberName = config.Services.MemberToName (Maked.fromMember prop) 
                            Index = config.Services.MemberToIndex (Maked.fromMember prop) 
                            TypeRef = field 
                            Summary = None
                            Extensions = Map.empty 
                        }
                    let field = config.Extensions |> List.fold (fun fld ex -> ex.ExtendProperty(prop, fld)) field
                    typeList, field :: fields

                let (typeList, fields) = props |> Seq.fold makeField (typeList, [])
                fields |> List.rev, typeList

            and generateType (typeList : TypeDeclaration list) (typ : Type) = 
                let toTypeMap (typeList : TypeDeclaration list) =
                    typeList 
                        |> Seq.map (fun p -> p.Type, p) 
                        |> Seq.collect 
                                (fun (p, v) -> 
                                    match p with
                                    | None -> []
                                    | Some a -> 
                                        match a with
                                        | Member t -> [t, Reference { RefName = v.TypeName; Type = t |> Some }]
                                        | _ -> [])
                        |> HashMap.fromSeq
                
                let makeSchema t =
                    { TypeName = config.Services.TypeToName (Member typ); Body = t; Type = Member typ |> Some; Summary = None; Extensions = Map.empty }

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
                    let reference, typeList = generateType typeList el
                    MayBe reference, typeList
                | IsArray el
                | IsArrayLike el -> 
                    let reference, typeList = generateType typeList el
                    TypeReference.Array reference, typeList
                | IsTuple els 
                | IsValueTuple els ->
                    match els with
                    | [] -> Unit, typeList
                    | _ ->
                        let pelem (refs, typeList) typ =
                            let ref, typeList = generateType typeList typ
                            ref :: refs, typeList 
                        let (refs, typeList) = els |> List.fold pelem ([], typeList)
                        TypeReference.Tuple (refs |> List.rev), typeList
                | IsEnum -> 
                        let es = makeEnumSchema Configuration.Default version typ |> TypeDefinition.EnumType 
                        let es = 
                            {
                                TypeName = config.Services.TypeToName (Member typ) 
                                Body = es
                                Type = Maked.fromType typ |> Some 
                                Summary = None 
                                Extensions = Map.empty
                            }
                        Reference { RefName = es.TypeName; Type = Some typ }, es :: typeList
                | IsUnion ul ->
                    let unionId = config.Services.TypeToName (Maked.fromType typ)
                    let unionRef = Reference { RefName = unionId; Type = Some typ }
                    let unionDummy = 
                        { 
                            TypeName = unionId 
                            Body = Dummy 
                            Type = Some(Member typ)  
                            Summary = config.Services.DocReader (Maked.fromMember typ) 
                            Extensions = Map.empty 
                        }
                    let typeList =  unionDummy :: typeList
                    let caseFolder (typeList, cases) (caseInfo : FSharp.Reflection.UnionCaseInfo)  =
                        if config.Services.IsInVersion version (Maked.fromCase caseInfo) |> not then typeList, cases
                        else
                            let caseId = caseInfo |> Maked.fromCase |> config.Services.MemberToName
                            let flds = 
                                caseInfo.GetFields()
                                |> Seq.filter  (Maked.fromMember >> config.Services.IsInVersion version)
                                |> Seq.toList
                            let typeList, caseTypeRef =
                                match flds with
                                | [] -> typeList, TypeReference.Unit
                                | [h] -> 
                                         let (r, typeList) = generateType typeList (h.PropertyType)
                                         typeList, r
                                | _ -> 
                                    let dummyMember = 
                                        { 
                                            TypeName = caseInfo |> Maked.fromCase |> config.Services.TypeToName
                                            Body = Dummy
                                            Type = Some(UnionCase caseInfo)  
                                            Summary = config.Services.DocReader (Maked.fromCase caseInfo)  
                                            Extensions = Map.empty 
                                        }
                                    let (fl, typeList) = makeTypeByProps typeList flds 
                                    let r = extendType (UnionCase caseInfo) { dummyMember with Body = MapType (None, fl) }
                                    
                                    let typeList = typeList |> List.map (fun p -> if p = dummyMember then r else p) 
                                    typeList, Reference { RefName = dummyMember.TypeName; Type = None }
                                        
                                    
                                    
                            let case = { 
                                MemberName = caseId 
                                Index = caseInfo |> Maked.fromCase |> config.Services.MemberToIndex 
                                TypeRef = caseTypeRef
                                Summary = config.Services.DocReader (Maked.fromCase caseInfo)
                                Extensions = Map.empty 
                            }
                            let case = config.Extensions |> List.fold (fun st p -> p.ExtendUnionCase(Maked.fromCase caseInfo, st)) case
                            typeList, case :: cases
                    let (typeList, cases) = 
                        ul |> List.fold caseFolder (typeList, [])
                    let unionNode =
                        { 
                            TypeName = unionDummy.TypeName 
                            Body = cases |> List.rev |> OneOfType 
                            Type = unionDummy.Type 
                            Summary = unionDummy.Summary 
                            Extensions = Map.empty 
                        }
                    let unionNode = extendType (Member typ) unionNode 
                    unionRef, 
                        typeList 
                        |> List.map (fun p -> if p = unionDummy then unionNode else p)

                | IsStruct -> 
                    let props = typ.GetProperties()
                    let structId = config.Services.TypeToName (Maked.fromType typ)
                    let structRef = Reference { RefName = structId; Type = Some typ }
                    let dummy = 
                        { 
                            TypeName = structId 
                            Body = Dummy 
                            Type = Some(Member typ)  
                            Summary = config.Services.DocReader (Maked.fromMember typ) 
                            Extensions = Map.empty 
                        }
                    let (fl, typeList) = makeTypeByProps (dummy :: typeList) props
                    let schema = extendType (Member typ) { dummy with Body = MapType (None, fl) }
                    structRef, typeList |> List.map (fun p -> if p = dummy then schema else p)

                | IsClass ->
                    let parent = typ.BaseType
                    let assembly = typ.Assembly
                    let directChieldren = assembly.GetTypes() |> Seq.filter (fun p -> p.IsClass && p.BaseType.Equals(typ)) |> Seq.toList
                    let declaredProps = typ.GetTypeInfo().DeclaredProperties |> Seq.toList
                    let classId = config.Services.TypeToName (Maked.fromType typ)
                    let dummy =
                        {
                            TypeName = classId
                            Body = Dummy
                            Type = Some(Member typ)
                            Summary = config.Services.DocReader (Maked.fromMember typ)
                            Extensions = Map.empty
                        }
                    let typeList = dummy :: typeList
                    let typeList, parentRef = 
                        if Unchecked.equals parent typeof<obj> |> not then
                            let (parentR, typeList) = generateType typeList parent
                            let parentRef =
                                match parentR with
                                | Reference nr -> nr
                                | _ -> invalidOp "Parent object is not reference object"
                            typeList, Some parentRef
                        else
                            typeList, None
                    
                    let regChieldren (tl, cl) ch =
                        let (r, ntl) = generateType tl ch
                        ntl, HashMap.add ch r cl
                    let typeList, chieldRefs = directChieldren |> List.fold regChieldren (typeList, HashMap.empty)

                    let (fl, typeList) = makeTypeByProps typeList declaredProps
                    let schema = { dummy with Body = MapType(parentRef, fl) }
                    let schema =
                        match declaredProps with
                        | [] -> 
                            match directChieldren with
                            | [] ->
                                match parentRef with
                                | Some pr ->
                                    {
                                        dummy with
                                            Body = 
                                                [{  
                                                    MemberName = Identifier.parse "Value"
                                                    Index = None
                                                    Extensions = Map.empty
                                                    TypeRef = Reference pr
                                                    Summary = None
                                                }] |> OneOfType
                                    }
                                | _ -> schema
                            | _ -> schema
                        | _ -> schema
                    let schema = extendType (Member typ) schema
                    Reference { RefName = schema.TypeName; Type = typ |> Some },         
                        typeList |> List.map (fun p -> if p = dummy then schema else p)
                    
                | _ ->  sprintf "'%A' - invalid schema member type" typ |> invalidArg "typ"
            generateType typeList typ

                    

            

                
                


        

    //let generateFromTypes endpoints wellknown version schemaName = ()
           
