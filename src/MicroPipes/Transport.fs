namespace MicroPipes.Transports
(*
type SDing<'t> =
    abstract Ding  : dinger :'t -> string

[<Struct>]
type DingInt =
    interface SDing<int> with
        member __.Ding a = a.ToString()


module Ding =
    let print<'tr, 't  when 'tr : struct  and 'tr : (new: unit -> 'tr) and 'tr :> SDing<'t>>  (a: 't) =
        printfn "%s" (Unchecked.defaultof<'tr>.Ding(a)) 

module Check =
    let abc a = Ding.print<DingInt, _> a
  

type SMonoid<'t> =
    abstract Null : 't
    abstract Sum : 't -> 't -> 't

module Adding =
    [<Struct>]
    type AddMonoid = 
        interface SMonoid<int> with
            member this.Null = 0
            member this.Sum a b = a + b

module Multiply = 
    [<Struct>]
    type ProductMonoid = 
        interface SMonoid<int> with
            member this.Null = 1
            member this.Sum a b = a * b
    
module Proc =
    let reduce<'tr, 't when 'tr : struct  and 'tr : (new: unit -> 'tr) and 'tr :> SMonoid<'t>> (a: 't seq) =
        let instance = Unchecked.defaultof<'tr>;
        a |> Seq.fold (instance.Sum) (instance.Null)

//let a = Proc.reduce<Multiply.ProductMonoid, _> [1;2;3;4]
//let b = Proc.reduce<Adding.AddMonoid, _> [1;2;3;4]
  *)
