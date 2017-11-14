namespace HeavenPipe

open System
open System.Threading.Tasks


type EventDeclaration = EventDeclaration

type EventPublishOptions = EventPublishOptions

type IEventPublisher<'event> =
    abstract PublishAsync : 'event * Func<EventPublishOptions, EventPublishOptions> -> Task
    
type CallDeclaration = CallDeclaration  

type IPortalServer = 
    abstract DeclareEvent<'event> :  EventDeclaration -> IEventPublisher<'event>
    //abstract DeclareCall<'request, 'response> : CallDeclaration * Func<  

type PortalServer = PortalServer
