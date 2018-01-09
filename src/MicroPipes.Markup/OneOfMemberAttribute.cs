using System;

namespace MicroPipes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OneOfMemberAttribute : Attribute
    {
        public OneOfMemberAttribute(string name = null)
        {
            Name = name != null ? Identifier.Parse(name) : null;
        }

        public Identifier Name { get; }
    }
}