module Tests

open Expecto
open Expecto.Flip
open MicroPipes.Schema

[<Tests>]
let tests =
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