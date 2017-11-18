module Schemas

open Expecto
open HeavenPipe.Schemas.Literals

module Result =
    let isError r =
        match r with
        | Error _ -> true
        | _ -> false
        
[<Tests>]
let tests =
    testList "shemas" [
        testCase "identifier correct" <| fun _ ->
           Expect.equal ((Identifier.Create "Hellow").ToString()) "Hellow" "Identifier creating"
        testCase "identifier incorrect" <| fun _ ->
           Expect.isTrue (Identifier.TryCreate "1Hellow" |> Result.isError) "Bad identifier"
        testCase "qualified identifier correct" <| fun _ ->
            Expect.equal ((QualifiedIdentifier.Create "One.Two").ToString()) "One.Two" "Qualified creating"
        testCase "qualified identifier incorrect" <| fun _ ->
            Expect.isTrue (QualifiedIdentifier.TryCreate "One..Two" |> Result.isError) "Bad qualified"    
    ]