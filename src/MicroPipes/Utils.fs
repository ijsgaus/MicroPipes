namespace MicroPipes
open System
open System.Collections.Generic
open System.Linq
open MicroPipes.Schema


type HashMap<'key, 'value> = LanguageExt.HashMap<'key, 'value>

module HashMap =
    let fromSeq<'key, 'value> (list : ('key * 'value) seq) =
        LanguageExt.HashMap.createRange<'key, 'value>(list)
    let tryGet key (map : HashMap<'key, 'value>) =
        LanguageExt.FSharp.fs(LanguageExt.HashMap.find(map, key)) 

module TypePatterns =

    let (|IsU8|_|) (t: Type)  =
        if Object.Equals(typeof<byte>, t) then Some() else None
    let (|IsI8|_|)  (t:Type) =
        if Object.Equals(typeof<sbyte>, t) then Some() else None
    let (|IsU16|_|)  (t:Type) =
        if Object.Equals(typeof<uint16>, t) then Some() else None
    let (|IsI16|_|)  (t:Type) =
        if Object.Equals(typeof<int16>, t) then Some() else None
    let (|IsU32|_|) (t: Type)  =
        if Object.Equals(typeof<uint32>, t) then Some() else None
    let (|IsI32|_|)  (t:Type) =
        if Object.Equals(typeof<int32>, t) then Some() else None
    let (|IsU64|_|)  (t:Type) =
        if Object.Equals(typeof<uint64>, t) then Some() else None
    let (|IsI64|_|)  (t:Type) =
        if Object.Equals(typeof<int64>, t) then Some() else None
    let (|IsF32|_|)  (t:Type) =
        if Object.Equals(typeof<float32>, t) then Some() else None
    let (|IsF64|_|)  (t:Type) =
        if Object.Equals(typeof<float>, t) then Some() else None
    let (|IsString|_|)  (t:Type) =
        if Object.Equals(typeof<string>, t) then Some() else None
    let (|IsUuid|_|)  (t:Type) =
        if Object.Equals(typeof<Guid>, t) then Some() else None
    let (|IsDT|_|)  (t:Type) =
        if Object.Equals(typeof<DateTime>, t) then Some() else None
    let (|IsDTO|_|)  (t:Type) =
        if Object.Equals(typeof<DateTimeOffset>, t) then Some() else None
    let (|IsTS|_|)  (t:Type) =
        if Object.Equals(typeof<TimeSpan>, t) then Some() else None
    let (|IsBool|_|)  (t:Type) =
        if Object.Equals(typeof<bool>, t) then Some() else None
    let (|IsUrl|_|)  (t:Type) =
        if Object.Equals(typeof<Uri>, t) then Some() else None

    let (|IsUnit|_|)  (t:Type) =
        if Object.Equals(typeof<unit>, t) then Some() else None
    
    let (|IsArray|_|) (t:Type) =
        if t.IsArray then Some(t.GetElementType()) else None
    
    let (|IsArrayLike|_|) (t: Type) =
        if t.IsArray |> not && Object.Equals(typeof<string>, t) |> not && t.IsConstructedGenericType then
            let findEnumerable (p: Type) =
                p.IsConstructedGenericType && Object.Equals(p.GetGenericTypeDefinition(), typedefof<IEnumerable<_>>)  
            match t.GetInterfaces() |> Seq.tryFind findEnumerable with
            | Some i -> i.GenericTypeArguments.[0] |> Some
            | None -> None  
        else
            None 

    let (|IsOption|_|) (t: Type) =
        if t.IsConstructedGenericType && Object.Equals(t.GetGenericTypeDefinition(), typedefof<Option<_>>) then 
            t.GenericTypeArguments.[0] |> Some
        else None

    let (|IsLEOption|_|) (t: Type) =
        if t.IsConstructedGenericType && Object.Equals(t.GetGenericTypeDefinition(), typedefof<LanguageExt.Option<_>>) then 
            t.GenericTypeArguments.[0] |> Some
        else None        

    let (|IsNullable|_|) (t: Type) =
        if t.IsConstructedGenericType && Object.Equals(t.GetGenericTypeDefinition(), typedefof<Nullable<_>>) then 
            t.GenericTypeArguments.[0] |> Some
        else None        
    
    let (|IsOptionLike|_|) (t: Type) =
        match t with
        | IsOption a | IsLEOption a | IsNullable a -> a |> Some
        | _ -> None

    let private tupleTypes = 
        [
            typedefof<Tuple<_>>; typedefof<Tuple<_,_>>; typedefof<Tuple<_,_,_>>; typedefof<Tuple<_,_,_,_>>
            typedefof<Tuple<_,_,_,_,_>>; typedefof<Tuple<_,_,_,_,_,_>>; typedefof<Tuple<_,_,_,_,_,_,_>>; typedefof<Tuple<_,_,_,_,_,_,_,_>>
        ]
    
    let private vTupleTypes = 
        [
            typedefof<ValueTuple<_>>; typedefof<ValueTuple<_,_>>; typedefof<ValueTuple<_,_,_>>; typedefof<ValueTuple<_,_,_,_>>
            typedefof<ValueTuple<_,_,_,_,_>>; typedefof<ValueTuple<_,_,_,_,_,_>>; typedefof<ValueTuple<_,_,_,_,_,_,_>> 
            typedefof<ValueTuple<_,_,_,_,_,_,_,_>>
        ]
    let (|IsTuple|_|) (t: Type) =
        if Object.Equals(t, typeof<Tuple>) then [] |> Some
        else 
        if t.IsConstructedGenericType |> not then None
        else
            let gt = t.GetGenericTypeDefinition()
            if tupleTypes |> List.tryFind (fun p -> Object.Equals(p, gt)) |> Option.isNone then
                t.GenericTypeArguments |> Array.toList |> Some
            else
                None
    
    let (|IsValueTuple|_|) (t: Type) =
        if Object.Equals(t, typeof<ValueTuple>) then [] |> Some
        else 
        if t.IsConstructedGenericType |> not then None
        else
            let gt = t.GetGenericTypeDefinition()
            if vTupleTypes |> List.tryFind (fun p -> Object.Equals(p, gt)) |> Option.isNone then
                t.GenericTypeArguments |> Array.toList |> Some
            else
                None

    let (|IsKnown|_|) map (t:Type) =
        map |> HashMap.tryGet t 

    let (|IsEnum|_|) (t:Type) = match t.IsEnum with | true -> Some() | _ -> None

    let (|IsUnion|_|) (t:Type) =
        match Reflection.FSharpType.IsUnion t with
        | false -> None
        | true -> 
            Reflection.FSharpType.GetUnionCases(t) |> Array.toList |> Some
                
        
    