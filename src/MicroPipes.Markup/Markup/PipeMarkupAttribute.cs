using System;
using System.Collections.Generic;
using MicroPipes.Reflection;

namespace MicroPipes.Markup
{
    public abstract class PipeMarkupAttribute : Attribute
    {
        public abstract IEnumerable<KeyValuePair<QualifiedIdentifier, object>> GetMetadata();
    }
}