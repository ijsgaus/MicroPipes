module Tests

open Expecto
open Expecto.Flip
open MicroPipes
open MicroPipes.Schema
open MicroPipes.Schema.Green
open NuGet.Versioning

[<Tests>]
let validations =
  testList "validations" [
    testCase "validate enum success" <| fun _ ->
      let en = {
        IsFlag = false;
        Values = 
          [
            { Name = "First" |> Identifier.parse; Value = 1; Summary = None  }
            { Name = "Second" |> Identifier.parse; Value = 2; Summary = None  }
            { Name = "Third" |> Identifier.parse; Value = 3; Summary = None  }
          ]
      }
      Expect.equal "Valid enum" [] (Validation.validateEnum en)
    testCase "validate enum failure" <| fun _ ->
      let en = {
        IsFlag = false;
        Values = 
          [
            { Name = "First" |> Identifier.parse; Value = 1; Summary = None  }
            { Name = "Second" |> Identifier.parse; Value = 2; Summary = None  }
            { Name = "Second" |> Identifier.parse; Value = 3; Summary = None  }
          ]
      }
      Expect.equal "Valid enum failure" [ "Duplicate enum field name 'Second'" ] (Validation.validateEnum en)
    testCase "validate enum type success" <| fun _ ->
      let en = 
        {
          IsFlag = false;
          Values = 
            [
              { Name = "First" |> Identifier.parse; Value = 1; Summary = None  }
              { Name = "Second" |> Identifier.parse; Value = 2; Summary = None  }
              { Name = "Third" |> Identifier.parse; Value = 3; Summary = None  }
            ]
        } |> Enum32
      Expect.equal "Valid enum type" [] (Validation.validateEnumType en)
    testCase "validate enum type failure" <| fun _ ->
      let en = 
        {
          IsFlag = false;
          Values = 
            [
              { Name = "First" |> Identifier.parse; Value = 1us; Summary = None  }
              { Name = "Second" |> Identifier.parse; Value = 2us; Summary = None  }
              { Name = "Second" |> Identifier.parse; Value = 3us; Summary = None  }
            ]
        } |> Enum16u
      Expect.equal "Valid enum type failure" [ "Duplicate enum field name 'Second'" ] (Validation.validateEnumType en)
    testCase "validate named entry without indexes success" <| fun _ ->
      let en = 
        [
          { NamedEntry.Name = "First" |> Identifier.parse; Index = None; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Second" |> Identifier.parse; Index = None; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Third" |> Identifier.parse; Index = None; Type = Unit; Summary = None  }
        ]
      Expect.equal "Valid named entry without indexes success" [ ] (Validation.validateNamedEntryList en)
    testCase "validate named entry with indexes success" <| fun _ ->
      let en = 
        [
          { NamedEntry.Name = "First" |> Identifier.parse; Index = Some 1; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Second" |> Identifier.parse; Index = Some 2; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Third" |> Identifier.parse; Index = Some 3; Type = Unit; Summary = None  }
        ]
      Expect.equal "Valid named entry with indexes success" [ ] (Validation.validateNamedEntryList en)
    testCase "validate named entry missed indexes failure" <| fun _ ->
      let en = 
        [
          { NamedEntry.Name = "First" |> Identifier.parse; Index = Some 1; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Second" |> Identifier.parse; Index = None; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Third" |> Identifier.parse; Index = Some 3; Type = Unit; Summary = None  }
        ]
      Expect.equal "Valid named entry missed indexes failure" [ "Index not specified for 'Second'" ] (Validation.validateNamedEntryList en)
    testCase "validate named entry without indexes failure" <| fun _ ->
      let en = 
        [
          { NamedEntry.Name = "First" |> Identifier.parse; Index = None; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Second" |> Identifier.parse; Index = None; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Second" |> Identifier.parse; Index = None; Type = Unit; Summary = None  }
        ]
      Expect.equal "Valid named entry without indexes failure" [ "Duplicate member name 'Second'"] (Validation.validateNamedEntryList en)
    testCase "validate named entry with indexes failure" <| fun _ ->
      let en = 
        [
          { NamedEntry.Name = "First" |> Identifier.parse; Index = Some 1; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Second" |> Identifier.parse; Index = Some 1; Type = Unit; Summary = None  }
          { NamedEntry.Name = "Third" |> Identifier.parse; Index = Some 2; Type = Unit; Summary = None  }
        ]
      Expect.equal "Valid named entry with indexes failure" [ "Duplicate indexes 1 on member [\"First\"; \"Second\"]" ] (Validation.validateNamedEntryList en)
    
  ]


type Test1 = class end
[<Version("2.0.0")>]
type Test2 = 
  | One = 1us
  | [<Version("2.2.0")>] Two = 2us
[<Version("2.0.0", "2.1.0")>]
type Test3 = class end


[<Tests>]
let schemaGen =
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