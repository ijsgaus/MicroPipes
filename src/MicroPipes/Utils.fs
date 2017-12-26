namespace MicroPipes
open System
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
    let (|IsEnum|_|) (t:Type) =
        if t.IsEnum then Some() else None

    let (|IsInList|_|) lst extr (t:Type) =
        lst |> List.map (fun p -> extr p, p) |> List.tryFind (fun (t1 : Type, _) -> Object.Equals(t1, t)) |> Option.map snd