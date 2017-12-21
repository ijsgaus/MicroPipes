using System;
using MicroPipes.Schema.Green;

namespace MicroPipes.Schema
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


        public override Type DotNetType => Green.DotNetType.IfNoneDefault();


        public override bool IsWellKnown => true;


        public TypeSchema ElementType => Service.TypeById(Green.ElementId);

    }
}