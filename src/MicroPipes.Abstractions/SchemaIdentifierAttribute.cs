using System;
using MicroPipes.Schema;

namespace MicroPipes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class SchemaIdentifierAttribute : Attribute
    {
        public SchemaIdentifierAttribute(string name)
            => Name = QualifiedIdentifier.Parse(name);

        public QualifiedIdentifier Name { get; }
        
    }
}