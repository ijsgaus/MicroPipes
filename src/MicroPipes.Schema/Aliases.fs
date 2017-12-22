module MicroPipes.Schema.Aliases

open System
open System.Collections.Generic
open System.Collections.Immutable

type Eq<'t> = IEqualityComparer<'t>

type HashMap<'key, 'value> = ImmutableDictionary<'key, 'value>

type HashMap<'eq, 'key, 'value when 'eq : struct and 'eq :> Eq<'key>> = 
    private HashMap of ImmutableDictionary<'key, 'value>
        
module HashMap =
    let create<'eq, 'key, 'value when 'eq : struct and 'eq :> Eq<'key>> seq : HashMap<'eq, 'key, 'value> =
        let dict = ImmutableDictionary.CreateRange(Unchecked.defaultof<'eq>, seq |> Seq.map (fun (k, v) -> KeyValuePair(k, v)))
        HashMap dict
    let value hs = let (HashMap d) = hs in d
        