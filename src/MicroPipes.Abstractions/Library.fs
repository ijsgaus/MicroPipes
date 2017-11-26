namespace MicroPipes.Abstractions
open System
open System.Diagnostics
open System.Runtime.CompilerServices
open System.Threading.Tasks
open System.Xml.Linq


type IBodyContainer<'t> = 
    abstract Body : 't

type Acknowledge =
    | Ack = 1
    | Nack = 2
    | Requeue = 3
    
type IAckMessage =
    abstract Approve : Acknowledge -> unit
     
type Message<'t> =
    {
        Body: 't
    }
    interface IBodyContainer<'t> with
        member __.Body = __.Body
  
type EventMessage<'t> =
    {
        Body: 't
    }
    interface IBodyContainer<'t> with
        member __.Body = __.Body
 
    


type AckMessage<'t> =
    {
       Body : 't
       Approve : Acknowledge -> unit
    }
    interface IBodyContainer<'t> with
        member __.Body = __.Body
    interface IAckMessage with
        member __.Approve a = __.Approve a
        
type AckEventMessage<'t> =
    {
       Body : 't
       Approve : Acknowledge -> unit
    }
    interface IBodyContainer<'t> with
        member __.Body = __.Body
    interface IAckMessage with
        member __.Approve a = __.Approve a   


[<AbstractClass>]
type Event<'t> = class end
    
[<AbstractClass>]    
type Call<'p, 'r> = class end

[<AbstractClass>]    
type Call<'p1, 'p2, 'r> = class end

[<AbstractClass>]    
type Call<'p1, 'p2, 'p3, 'r> = class end

[<AbstractClass>]    
type Call<'p1, 'p2, 'p3, 'p4, 'r> = class end

[<AbstractClass>]    
type Call<'p1, 'p2, 'p3, 'p4, 'p5, 'r> = class end

[<AbstractClass>]    
type Call<'p1, 'p2, 'p3, 'p4, 'p5, 'p6, 'r> = class end
    
[<AbstractClass>]    
type Call<'p1, 'p2, 'p3, 'p4, 'p5, 'p6, 'p7,  'r> = class end
    
type AsyncEventParams =
    {
        Ttl : TimeSpan
    }    

type EventParams =
         {
             Ttl : TimeSpan
         }
         
type AcknowledgeExceptionPolicy = 
    abstract OnException : Exception -> Acknowledge         
         
type IAsyncEventProducer<'event, 'payload when 'event :> Event<'payload>> =
    abstract PublishAsync : 'payload *  AsyncEventParams -> Task

type IEventProducer<'event, 'payload when 'event :> Event<'payload>> = 
    abstract Publish : 'payload * EventParams -> unit
    
type IEventListener<'event, 'payload when 'event :> Event<'payload>> =
    abstract Subscribe : (EventMessage<'payload> * AcknowledgeExceptionPolicy -> Task<Acknowledge>) -> IDisposable 


          

    
