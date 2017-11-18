module Schemas

open Expecto
open HeavenPipe.Schemas.Literals

[<Tests>]
let tests =
    testList "shemas" [
        testCase "identifier correct" <| fun _ ->
           Expect.equal ((Identifier.Create "Hellow").ToString()) "Identifier \"Hellow\"" "Identifier creating"
        testCase "identifier incorrect" <| fun _ ->
           Expect.equal (Identifier.TryCreate "1Hellow") (Error "Invalid identifier") "Bad identifier"
    ]