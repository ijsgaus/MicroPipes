using System;
using MicroPipes.Schema;

namespace MicroPipes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MemberIdentifierAttribute : Attribute
    {
        public MemberIdentifierAttribute(string name) => Name = Identifier.Parse(name);

        public Identifier Name { get; }
    }
}