module FSharp.LanguageExt
open System


[<AutoOpen>]
module TypeClasses =
    type Eq<'t> = LanguageExt.TypeClasses.Eq<'t>

type HashMap<'eqk,'k, 'v when 'eqk : struct and 'eqk : (new: unit -> 'eqk) and 'eqk :> ValueType and 'eqk :> TypeClasses.Eq<'k>> 
    = LanguageExt.HashMap<'eqk, 'k, 'v> 

type HashSet<'eq,'t when 'eq : struct and 'eq : (new: unit -> 'eq) and 'eq :> ValueType and 'eq :> TypeClasses.Eq<'t>> 
    = LanguageExt.HashSet<'eq, 't>
//type HashMap<'k, 'v> = LanguageExt.HashMap<'k, 'v>
    
