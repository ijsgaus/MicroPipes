﻿using System;

namespace MicroPipes.Schema
{
    public class SchemaFormatException : SchemaException
    {
        public SchemaFormatException()
        {
        }

        public SchemaFormatException(string message) : base(message)
        {
        }

        public SchemaFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}