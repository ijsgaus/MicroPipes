namespace MicroPipes
open System
open System.Reflection

module Reflection =
    let getAttribute<'t when 't :> Attribute and 't: null> (mi : MemberInfo) =
        match mi.GetCustomAttribute<'t>() with
        | null -> None
        | a -> Some a
    let isType<'t> (t:Type) = if Object.Equals(typeof<'t>, t) then Some(t) else None

    let isEnumBaseType<'t> (t : Type) =
        match Enum.GetUnderlyingType(t) with
        | null -> None
        | p -> isType<'t> p |> Option.map (fun _ -> t)