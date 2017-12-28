namespace MicroPipes

[<AutoOpen>]
module OptionOperators =
    let (>=>) a b =
        fun x -> a x |> Option.bind b

    let (>->) a b =
        fun x -> a x |> Option.map b
            
    let (=|>) a b =
        fun x -> a x |> Option.orElseWith (fun () -> b x)
            
    let (?=) a b = 
        fun x -> a x |> Option.defaultWith (fun () -> b x)