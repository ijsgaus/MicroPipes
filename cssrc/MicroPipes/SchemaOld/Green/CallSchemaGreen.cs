﻿using System.Net.Mime;
using MicroPipes.Markup.RabbitMq;

namespace MicroPipes.SchemaOld.Green
{
    public class CallSchemaGreen : EndpointSchemaGreen
    {
        public CallSchemaGreen(string name, string codeName, int requestTypeId, int? responseTypeId = null,
            ContentType contentType = null, string routingKey = null, RequestQueueSchema requestQueue = null,
            ExchangeSchema exchange = null, ExchangeSchema responseExchange = null) : base(name, codeName, contentType,
            routingKey, exchange)
        {
            if(responseExchange != null && responseExchange.Type == ExchangeKind.Fanout)
                throw new SchemaException($"Cannot set fanount response exchange in endpoint {name}");
            ResponseExchange = responseExchange;
            RequestQueue = requestQueue;
            RequestTypeId = requestTypeId;
            ResponseTypeId = responseTypeId;
        }

        public CallSchemaGreen(CallSchemaGreen @base,string name, string codeName, int requestTypeId, int? responseTypeId = null,
            ContentType contentType = null, string routingKey = null, RequestQueueSchema requestQueue = null,
            ExchangeSchema exchange = null, ExchangeSchema responseExchange = null) : base(name, codeName, contentType,
            routingKey, exchange)
        {
            ResponseExchange = responseExchange;
            RequestQueue = requestQueue;
            RequestTypeId = requestTypeId;
            ResponseTypeId = responseTypeId;
        }

        public ExchangeSchema ResponseExchange { get; }
        public RequestQueueSchema RequestQueue { get; }
        public int RequestTypeId { get; }
        public int? ResponseTypeId { get; }
    }


}