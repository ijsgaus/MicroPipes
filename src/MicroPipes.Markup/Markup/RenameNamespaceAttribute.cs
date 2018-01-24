using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MicroPipes.Reflection;

namespace MicroPipes.Markup
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class RenameNamespaceAttribute : PipeMarkupAttribute
    {
        public string FromPrefix { get; }
        public string ToPrefix { get; }
        public int Order { get; set; }
        public static readonly QualifiedIdentifier RenameNamespace = "RenameNamespace";

        public RenameNamespaceAttribute([NotNull] string fromPrefix, [NotNull] string toPrefix)
        {
            if (string.IsNullOrWhiteSpace(fromPrefix))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fromPrefix));
            
            FromPrefix = fromPrefix;
            ToPrefix = toPrefix ?? throw new ArgumentNullException(nameof(toPrefix));
        }

        public override IEnumerable<KeyValuePair<QualifiedIdentifier, object>> GetMetadata()
        {
            return new Dictionary<QualifiedIdentifier, object>
            {
                { RenameNamespace, (FromPrefix, ToPrefix, Order) }
            };
        }
    }
}