﻿using MicroPipes.Markup.RabbitMq;

namespace MicroPipes.Schema
{
    public class ExchangeSchema 
    {
        public ExchangeSchema(string name, ExchangeKind type = ExchangeKind.Direct, bool durable = true,
            bool autoDelete = false, bool delayed = false, string alternate = null)
        {
            Name = name;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Delayed = delayed;
            Alternate = alternate;
        }

        public string Name { get; }
        public ExchangeKind Type { get; }
        public bool Durable { get; }
        public bool AutoDelete { get; }
        public bool Delayed { get; }
        public string Alternate { get; }


    }
}