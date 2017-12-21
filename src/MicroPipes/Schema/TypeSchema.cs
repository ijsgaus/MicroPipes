using System;

namespace MicroPipes.Schema
{
    public abstract class TypeSchema 
    {
        public abstract string ContractName { get; }
        public abstract string CodeName { get; }
        public abstract string SchemaName { get; }
        public abstract Type DotNetType { get; }
        public abstract bool IsWellKnown { get; }
    }
}