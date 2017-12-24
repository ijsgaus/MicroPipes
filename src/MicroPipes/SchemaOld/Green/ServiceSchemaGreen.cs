using System;
using System.Net.Mime;
using LanguageExt;
using MicroPipes.Markup.RabbitMq;

namespace MicroPipes.SchemaOld.Green
{
    public class ServiceSchemaGreen 
    {
        public ServiceSchemaGreen(string name, string owner, string codeName, HashMap<string, EventSchemaGreen> events,
            HashMap<string, CallSchemaGreen> calls, HashMap<int, TypeSchemaGreen> types, ContentType contentType = null,
            ExchangeSchema exchange = null, ExchangeSchema responseExchange = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(owner))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(owner));
            Name = name;
            Owner = owner;
            CodeName = codeName;
            Events = events;
            Calls = calls;
            ContentType = contentType;
            if(exchange != null && exchange.Type == ExchangeKind.Fanout)
                throw new SchemaException($"Invalid exchange type on service level in service {Name} - cannot be Fanout");
            Exchange = exchange;
            ResponseExchange = responseExchange;
            if(responseExchange != null && responseExchange.Type == ExchangeKind.Fanout)
                throw new SchemaException($"Invalid response exchange type on service level in service {Name} - cannot be Fanout");
            if (string.IsNullOrWhiteSpace(codeName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(codeName));
            Types = types;
        }

        public string Name { get; }
        public string Owner { get; }
        public string CodeName { get; }
        public ExchangeSchema Exchange { get; }
        public ExchangeSchema ResponseExchange { get; }
        public HashMap<string, EventSchemaGreen> Events { get; }
        public HashMap<string, CallSchemaGreen> Calls { get; }
        public HashMap<int, TypeSchemaGreen> Types { get; }
        public ContentType ContentType { get; }
    }

    
}