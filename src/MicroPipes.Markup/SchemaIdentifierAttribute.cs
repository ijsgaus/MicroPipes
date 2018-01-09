using System;

namespace MicroPipes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Method)]
    public class SchemaIdentifierAttribute : Attribute
    {
        public SchemaIdentifierAttribute(string name)
            => Name = QualifiedIdentifier.Parse(name);

        public QualifiedIdentifier Name { get; }
        
    }
}