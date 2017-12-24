﻿using System;
using LanguageExt;

namespace MicroPipes.SchemaOld.Green
{
    public class NullableTypeSchemaGreen : TypeSchemaGreen
    {
        public NullableTypeSchemaGreen(Option<Type> dotNetType, int elementTypeId) : base(dotNetType, true)
        {
            ElementTypeId = elementTypeId;
        }

        public NullableTypeSchemaGreen(TypeSchemaGreen @base, Option<Type> dotNetType, int elementTypeId) : base(@base, dotNetType, true)
        {
            ElementTypeId = elementTypeId;
        }

        public int ElementTypeId { get; }
    }
}