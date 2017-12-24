﻿using System;
using MicroPipes.SchemaOld.Green;

namespace MicroPipes.SchemaOld
{
    public sealed class ArrayTypeSchema : TypeSchema
    {
        public ServiceSchema Service { get; }
        private ArrayTypeSchemaGreen Green { get; }

        public ArrayTypeSchema(ServiceSchema service, ArrayTypeSchemaGreen green)
        {
            Service = service;
            Green = green;
        }

        public override string SchemaName => ElementType.SchemaName != null ? ElementType.SchemaName + "[]" : null;


        public override string CodeName => ElementType.CodeName != null ? ElementType.CodeName + "[]" : null;


        public override string ContractName => ElementType.ContractName != null ? ElementType.ContractName + "[]" : null;


        public override Type DotNetType => Green.DotNetType.Unwrap();


        public override bool IsWellKnown => true;


        public TypeSchema ElementType => Service.TypeById(Green.ElementId);

    }
}