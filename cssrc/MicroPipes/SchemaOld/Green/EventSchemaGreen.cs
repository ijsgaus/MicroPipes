using System.Net.Mime;

namespace MicroPipes.SchemaOld.Green
{
    public class EventSchemaGreen : EndpointSchemaGreen
    {
        public EventSchemaGreen(string name, string codeName, int typeId, ContentType contentType = null,
            string routingKey = null, ExchangeSchema exchange = null) : base(name, codeName, contentType, routingKey,
            exchange)
        {
            TypeId = typeId;
        }

        public EventSchemaGreen(EventSchemaGreen @base, string name, string codeName, int typeId, ContentType contentType = null,
            string routingKey = null, ExchangeSchema exchange = null) : base(@base, name, codeName, contentType, routingKey, exchange)
        {
            TypeId = typeId;
        }

        public int TypeId { get; }

    }
}