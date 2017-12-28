module Tests

open System
open Expecto
open Expecto.Flip
open MicroPipes
open MicroPipes.Schema
open MicroPipes.Schema.Green
open System.Collections.Generic
open NuGet.Versioning
open MicroPipes.Schema
open Mono.Cecil
open MicroPipes.Schema.SchemaGenerator

[<Tests>]
let validations =
  testList "validations" [
    testCase "validate enum success" <| fun _ ->
      let en = {
        IsFlag = false;
        Values = 
          [
            { FieldName = "First" |> Identifier.parse; Value = 1; Summary = None  }
            { FieldName = "Second" |> Identifier.parse; Value = 2; Summary = None  }
            { FieldName = "Third" |> Identifier.parse; Value = 3; Summary = None  }
          ]
        Extensions = Map.empty
      }
      Expect.equal "Valid enum" [] (Validation.validateEnum en)
    testCase "validate enum failure" <| fun _ ->
      let en = {
        IsFlag = false;
        Values = 
          [
            { FieldName = "First" |> Identifier.parse; Value = 1; Summary = None  }
            { FieldName = "Second" |> Identifier.parse; Value = 2; Summary = None  }
            { FieldName = "Second" |> Identifier.parse; Value = 3; Summary = None  }
          ]
        Extensions = Map.empty
       
      }
      Expect.equal "Valid enum failure" [ "Duplicate enum field name 'Second'" ] (Validation.validateEnum en)
    testCase "validate enum type success" <| fun _ ->
      let en = 
        {
          IsFlag = false;
          Values = 
            [
              { FieldName = "First" |> Identifier.parse; Value = 1; Summary = None  }
              { FieldName = "Second" |> Identifier.parse; Value = 2; Summary = None }
              { FieldName = "Third" |> Identifier.parse; Value = 3; Summary = None }
            ]
          Extensions = Map.empty
        } |> Enum32
      Expect.equal "Valid enum type" [] (Validation.validateEnumType en)
    testCase "validate enum type failure" <| fun _ ->
      let en = 
        {
          IsFlag = false;
          Values = 
            [
              { FieldName = "First" |> Identifier.parse; Value = 1us; Summary = None  }
              { FieldName = "Second" |> Identifier.parse; Value = 2us; Summary = None  }
              { FieldName = "Second" |> Identifier.parse; Value = 3us; Summary = None  }
            ]
          Extensions = Map.empty
        } |> Enum16u
      Expect.equal "Valid enum type failure" [ "Duplicate enum field name 'Second'" ] (Validation.validateEnumType en)
    testCase "validate named entry without indexes success" <| fun _ ->
      let en = 
        [
          { MemberName = "First" |> Identifier.parse; Index = None; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Second" |> Identifier.parse; Index = None; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Third" |> Identifier.parse; Index = None; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
        ]
      Expect.equal "Valid named entry without indexes success" [ ] (Validation.validateNamedEntryList en)
    testCase "validate named entry with indexes success" <| fun _ ->
      let en = 
        [
          { MemberName = "First" |> Identifier.parse; Index = Some 1; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Second" |> Identifier.parse; Index = Some 2; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Third" |> Identifier.parse; Index = Some 3; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
        ]
      Expect.equal "Valid named entry with indexes success" [ ] (Validation.validateNamedEntryList en)
    testCase "validate named entry missed indexes failure" <| fun _ ->
      let en = 
        [
          { MemberName = "First" |> Identifier.parse; Index = Some 1; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Second" |> Identifier.parse; Index = None; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Third" |> Identifier.parse; Index = Some 3; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
        ]
      Expect.equal "Valid named entry missed indexes failure" [ "Index not specified for 'Second'" ] (Validation.validateNamedEntryList en)
    testCase "validate named entry without indexes failure" <| fun _ ->
      let en = 
        [
          { MemberName = "First" |> Identifier.parse; Index = None; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Second" |> Identifier.parse; Index = None; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Second" |> Identifier.parse; Index = None; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
        ]
      Expect.equal "Valid named entry without indexes failure" [ "Duplicate member name 'Second'"] (Validation.validateNamedEntryList en)
    testCase "validate named entry with indexes failure" <| fun _ ->
      let en = 
        [
          { MemberName = "First" |> Identifier.parse; Index = Some 1; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Second" |> Identifier.parse; Index = Some 1; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
          { MemberName = "Third" |> Identifier.parse; Index = Some 2; TypeRef = Unit; Summary = None; Extensions = Map.empty  }
        ]
      Expect.equal "Valid named entry with indexes failure" [ "Duplicate indexes 1 on member [\"First\"; \"Second\"]" ] (Validation.validateNamedEntryList en)
    
  ]


type Test1 = class end
[<VersionRange("[2.0.0,)")>]
type Test2 = 
  | One = 1us
  | [<VersionRange("[2.2.0,)")>] Two = 2us
[<VersionRange("[2.0.0, 2.1.0]")>]
type Test3 = class end

type Test4 =
  | First  of Version
  | Second of int


[<Tests>]
let schemaGen =
  let config = Configuration.Default
  testList "schema generation" [
    testCase "Is in version true 1" <| fun _ ->
      let ver = SemanticVersion.Parse("2.2.0")
      Expect.isTrue "Is in version true 1" (SchemaGenerator.isInVersion ver typeof<Test1>)
    testCase "Is in version true 2" <| fun _ ->
      let ver = SemanticVersion.Parse("2.0.0-alpha")
      Expect.isTrue "Is in version true 2" (SchemaGenerator.isInVersion ver typeof<Test2>)
    testCase "Is in version true 3" <| fun _ ->
      let ver = SemanticVersion.Parse("2.0.3")
      Expect.isTrue "Is in version true 3" (SchemaGenerator.isInVersion ver typeof<Test3>)
    testCase "Is in version false 1" <| fun _ ->
      let ver = SemanticVersion.Parse("1.0.0")
      Expect.isFalse "Is in version false 1" (SchemaGenerator.isInVersion ver typeof<Test2>)
    testCase "Is in version false 2" <| fun _ ->
      let ver = SemanticVersion.Parse("2.2.0-alpha")
      Expect.isFalse "Is in version false 2" (SchemaGenerator.isInVersion ver typeof<Test3>)
    testCase "Is in version false 3" <| fun _ ->
      let ver = SemanticVersion.Parse("2.1.1")
      Expect.isFalse "Is in version false 3" (SchemaGenerator.isInVersion ver typeof<Test3>)
    
    testCase "Is List<T> array like" <| fun _ ->
      let a = typeof<List<int>>
      match a with
      | TypePatterns.IsArrayLike i -> Expect.equal "Must be int" i (typeof<int>)
      | _ -> Tests.failtest "No array detected"

    testCase "Schema u8" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<byte>
      Expect.equal "Empty list" [] s
      Expect.equal "U8 ref" (Basic(Ordinal(U8))) r
    testCase "Schema i8" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<sbyte>
      Expect.equal "Empty list" [] s
      Expect.equal "I8 ref" (Basic(Ordinal(I8))) r
    testCase "Schema u16" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<uint16>
      Expect.equal "Empty list" [] s
      Expect.equal "U16 ref" (Basic(Ordinal(U16))) r
    testCase "Schema i16" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<int16>
      Expect.equal "Empty list" [] s
      Expect.equal "I16 ref" (Basic(Ordinal(I16))) r
    testCase "Schema u32" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<uint32>
      Expect.equal "Empty list" [] s
      Expect.equal "U32 ref" (Basic(Ordinal(U32))) r
    testCase "Schema i32" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<int>
      Expect.equal "Empty list" [] s
      Expect.equal "I32 ref" (Basic(Ordinal(I32))) r
    testCase "Schema u64" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<uint64>
      Expect.equal "Empty list" [] s
      Expect.equal "U64 ref" (Basic(Ordinal(U64))) r
    testCase "Schema i64" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<int64>
      Expect.equal "Empty list" [] s
      Expect.equal "I64 ref" (Basic(Ordinal(I64))) r

    testCase "Schema F32" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<float32>
      Expect.equal "Empty list" [] s
      Expect.equal "F32 ref" (Basic(Float(F32))) r
    testCase "Schema F64" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<float>
      Expect.equal "Empty list" [] s
      Expect.equal "F64 ref" (Basic(Float(F64))) r

    testCase "Schema string" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<string>
      Expect.equal "Empty list" [] s
      Expect.equal "String ref" (Basic(BasicType.String)) r
    testCase "Schema Uuid" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<Guid>
      Expect.equal "Empty list" [] s
      Expect.equal "Uuid ref" (Basic(Uuid)) r
    testCase "Schema DT" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<DateTime>
      Expect.equal "Empty list" [] s
      Expect.equal "DT ref" (Basic(DT)) r
    testCase "Schema DTO" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<DateTimeOffset>
      Expect.equal "Empty list" [] s
      Expect.equal "DTO ref" (Basic(DTO)) r
    testCase "Schema TS" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<TimeSpan>
      Expect.equal "Empty list" [] s
      Expect.equal "TS ref" (Basic(TS)) r
    testCase "Schema Bool" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<bool>
      Expect.equal "Empty list" [] s
      Expect.equal "Bool ref" (Basic(Bool)) r    
    testCase "Schema Url" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<Uri>
      Expect.equal "Empty list" [] s
      Expect.equal "Url ref" (Basic(Url)) r

    testCase "Schema known" <| fun _ ->
      let wki = Identifier.parseQualified "Version" 
      let wk =
        [
          { 
            TypeName = wki 
            Body = Wellknown None
            Type = typeof<Version> |> Some
            Summary = None  
            Extensions = Map.empty            
          }
        ]
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") wk typeof<Version>
      Expect.equal "Empty list" wk s
      Expect.equal "Known ref" (TypeReference.Reference { RefName = wki; Type = Some typeof<Version> }) r

    testCase "Array known" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<byte array>
      Expect.equal "Empty list" [] s
      Expect.equal "Array ref" (TypeReference.Array (U8 |> Ordinal |> Basic)) r
    testCase "List known" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<byte list>
      Expect.equal "Empty list" [] s
      Expect.equal "Array ref" (TypeReference.Array (U8 |> Ordinal |> Basic)) r

    testCase "Option known" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<byte option>
      Expect.equal "Empty list" [] s
      Expect.equal "MayBe ref" (TypeReference.MayBe (U8 |> Ordinal |> Basic)) r
    testCase "Nullable known" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<Nullable<byte>>
      Expect.equal "Empty list" [] s
      Expect.equal "MayBe ref" (TypeReference.MayBe (U8 |> Ordinal |> Basic)) r
    testCase "LanguageExt Option known" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<LanguageExt.Option<byte>>
      Expect.equal "Empty list" [] s
      Expect.equal "MayBe ref" (TypeReference.MayBe (U8 |> Ordinal |> Basic)) r

    testCase "List<Option<byte>> known" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<List<byte option>>
      Expect.equal "Empty list" [] s
      Expect.equal "Array of MayBe ref" (TypeReference.Array(MayBe (U8 |> Ordinal |> Basic))) r

    testCase "Tuple known" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<byte * int>
      Expect.equal "Empty list" [] s
      Expect.equal "Tuple ref" (TypeReference.Tuple 
                                    [
                                      U8 |> Ordinal |> Basic 
                                      I32 |> Ordinal |> Basic
                                    ]) r

    testCase "ValueTuple<,> known" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<struct (byte * int)>
      Expect.equal "Empty list" [] s
      Expect.equal "Tuple ref" (TypeReference.Tuple [U8 |> Ordinal |> Basic; I32 |> Ordinal |> Basic]) r

    testCase "ValueTuple known as Unit" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<ValueTuple>
      Expect.equal "Empty list" [] s
      Expect.equal "Tuple ref" (TypeReference.Unit) r
    testCase "Unit known as Unit" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "1.0.0") [] typeof<unit>
      Expect.equal "Empty list" [] s
      Expect.equal "Tuple ref" (TypeReference.Unit) r

    testCase "Enum with version small then field" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "2.0.0") [] typeof<Test2>
      let id = "Tests.Test2" |> Identifier.parseQualified
      let ts = 
        {
          TypeName = id
          Body =  
            {
                IsFlag = false
                Values = 
                  [ 
                    { FieldName = "One" |> Identifier.parse; Value = 1us; Summary = None}
                  ]
                Extensions = Map.empty
            } |> Enum16u |> EnumType
          Type = typeof<Test2> |> Some
          Summary = None
          Extensions = Map.empty  
        }
      Expect.equal "Empty list" [ ts ] s
      Expect.equal "Tuple ref" (TypeReference.Reference { RefName = id; Type = Some typeof<Test2> }) r


    testCase "Enum with version greater then field" <| fun _ ->
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "3.0.0") [] typeof<Test2>
      let id = "Tests.Test2" |> Identifier.parseQualified
      let ts = 
        {
          TypeName = id
          Body =  
            {
                IsFlag = false
                Values = 
                  [ 
                    { FieldName = "One" |> Identifier.parse; Value = 1us; Summary = None}
                    { FieldName = "Two" |> Identifier.parse; Value = 2us; Summary = None}
                  ]
                Extensions = Map.empty
            } |> Enum16u |> EnumType
          Type = typeof<Test2> |> Some
          Summary = None
          Extensions = Map.empty
        }
      Expect.equal "Empty list" [ ts ] s
      Expect.equal "Tuple ref" (TypeReference.Reference { RefName = id; Type = Some typeof<Test2> }) r

    testCase "FSharp union" <| fun _ ->
      let wki = Identifier.parseQualified "Version" 
      let wk =
          { 
            TypeName = wki 
            
            Body = Wellknown None
            Type = typeof<Version> |> Some
            Summary = None  
            Extensions = Map.empty  
          }
      let r, s = SchemaGenerator.generateType config (SemanticVersion.Parse "3.0.0") [wk] typeof<Test4>
      let id = "Tests.Test4" |> Identifier.parseQualified
      
      let ts = 
        {
          TypeName = id
          
          Body =  
            [ 
              { MemberName = "First" |> Identifier.parse; Index = None; Summary = None; TypeRef = Reference { RefName = wki; Type = typeof<Version> |> Some }; Extensions = Map.empty}
              { MemberName = "Second" |> Identifier.parse; Index = None; Summary = None; TypeRef = I32 |> Ordinal |> Basic; Extensions = Map.empty }
            ] |> OneOfType
          Type = typeof<Test4> |> Some
          Summary = None
          Extensions = Map.empty
        }
        
      Expect.equal "Empty list" [ ts; wk ] s
      Expect.equal "OneOf ref" (TypeReference.Reference { RefName = id; Type = Some typeof<Test4> }) r  
  ]

[<Tests>]
let identifiers =
  testList "idenifiers" [
      testCase "must parse valid identifier" <| fun _ ->
        Expect.equal "Valid identifier" "Abcd_ef13" ((Identifier.parse "Abcd_ef13").ToString())
      testCase "must throw on invalid identifier" <| fun _ ->
        Expect.throws "Invalid identifier" (fun () -> Identifier.parse "12_Abcd_ef13" |> ignore)

      testCase "must parse valid qualified identifier" <| fun _ ->
        Expect.equal "Valid identifier" "Abcd.ef13" ((Identifier.parseQualified "Abcd.ef13").ToString())

      testCase "must throw on invalid qualified identifier" <| fun _ ->
        Expect.throws "Invalid identifier" (fun () -> Identifier.parseQualified "Abcd..ef13" |> ignore)

      testCase "must get qulified identifier name part" <| fun _ ->
        let id = Identifier.parseQualified "Hellow.The.Best.World"
        let name = Identifier.parse "World"
        Expect.equal "Must equal" id.Name name

      testCase "must get qulified identifier namespace part" <| fun _ ->
        let id = Identifier.parseQualified "Hellow.The.Best.World"
        let nameSpace = Identifier.parseQualified "Hellow.The.Best" |> Some
        Expect.equal "Must equal" (id |> Identifier.nameSpace) nameSpace

      testCase "case insentivity identifier comparison" <| fun _ ->
        let id1 = Identifier.parse "Hellow"
        let id2 = Identifier.parse "hellow"
        Expect.equal "Case insenrivity" id1 id2

      testCase "case insentivity qualified identifier comparison" <| fun _ ->
        let id1 = Identifier.parseQualified "Hellow.worlD"
        let id2 = Identifier.parseQualified "hellow.World"
        Expect.equal "Case insenrivity" id1 id2
    (*testCase "universe exists (╭ರᴥ•́)" <| fun _ ->
      let subject = true
      Expect.isTrue subject "I compute, therefore I am."

    testCase "when true is not (should fail)" <| fun _ ->
      let subject = false
      Expect.isTrue subject "I should fail because the subject is false"

    testCase "I'm skipped (should skip)" <| fun _ ->
      Tests.skiptest "Yup, waiting for a sunny day..."

    testCase "I'm always fail (should fail)" <| fun _ ->
      Tests.failtest "This was expected..."

    testCase "contains things" <| fun _ ->
      Expect.containsAll [| 2; 3; 4 |] [| 2; 4 |]
                         "This is the case; {2,3,4} contains {2,4}"

    testCase "contains things (should fail)" <| fun _ ->
      Expect.containsAll [| 2; 3; 4 |] [| 2; 4; 1 |]
                         "Expecting we have one (1) in there"

    testCase "Sometimes I want to ༼ノಠل͟ಠ༽ノ ︵ ┻━┻" <| fun _ ->
      Expect.equal "abcdëf" "abcdef" "These should equal"

    test "I am (should fail)" {
      "╰〳 ಠ 益 ಠೃ 〵╯" |> Expect.equal true false 
    }*)
]