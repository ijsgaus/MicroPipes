using System;

namespace MicroPipes.SchemaOld
{
    public class SchemaException : Exception
    {
        public SchemaException()
        {
        }

        public SchemaException(string message) : base(message)
        {
        }

        public SchemaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}