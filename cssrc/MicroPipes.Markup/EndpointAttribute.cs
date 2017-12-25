using System;

namespace MicroPipes.Markup
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EndpointAttribute : Attribute
    {
        public EndpointAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}