namespace MicroPipes
open System
open System.Threading
open System.Threading.Tasks

type Acknowledge =
    | Accept = 0
    | Reject = 1
    | Retry = 2

type SourceId = | SourceId of string
type CorrelationId = | CorrelationId of string
type ReplyTo = | ReplyTo of string



type EventMessage<'event> = {
    Payload : 'event
    CorrelationId : CorrelationId option
    Sender : SourceId option
}

type Request<'request> = {
    Request : 'request
    Sender : SourceId option
    ReplyTo : ReplyTo
    CorrelationId : CorrelationId option
}

type Response<'response> = {
    Response : 'response
    Sender : SourceId option
    ReplyTo : ReplyTo
    CorrelationId : CorrelationId option
}

type IEventProducer<'event> =
    abstract PublishAsync : msg: EventMessage<'event> -> Task

type IEventConsumer<'event> =
    abstract HandleAsync : msg: EventMessage<'event> * cancellation : CancellationToken -> Task<Acknowledge>


type IRequestHandler<'request, 'response> =
    abstract HandleAsync : request: Request<'request> * cancellation : CancellationToken -> Task<'response>


type IResponseSender<'response> =
    abstract ResponseAsync : response : Response<'response> -> Task

type IRequestReceiver<'request, 'sender, 'response when 'sender :> IResponseSender<'response>> =
    abstract HandleAsync : request: Request<'request> * responder : 'sender * cancellation : CancellationToken -> Task<Acknowledge>

type IRequestReceiver<'request, 'response> =
    inherit IRequestReceiver<'request, IResponseSender<'response>,'response>

type IResponseReceiver<'response> =
    abstract HandleAsync : response : Func<Response<'response>> * cancellation : CancellationToken -> Task<Acknowledge>

type IRequestSender<'request> =
    abstract SendAsync : request : Request<'request> -> Task
