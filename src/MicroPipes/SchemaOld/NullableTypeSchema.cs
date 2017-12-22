using System;
using MicroPipes.SchemaOld.Green;

namespace MicroPipes.SchemaOld
{
    public class NullableTypeSchema : TypeSchema
    {
        public ServiceSchema Service { get; }
        private readonly NullableTypeSchemaGreen _green;

        public NullableTypeSchema(ServiceSchema service, NullableTypeSchemaGreen green)
        {
            Service = service;
            _green = green;
        }

        public override string ContractName => ElementType.ContractName != null ? ElementType.ContractName + "?" : null;


        public override string CodeName => ElementType.CodeName != null ? ElementType.CodeName + "?" : null;


        public override string SchemaName => ElementType.SchemaName != null ? ElementType.SchemaName + "?" : null;


        public override Type DotNetType => _green.DotNetType.IfNoneDefault();


        public override bool IsWellKnown => true;

        public TypeSchema ElementType => Service.TypeById(_green.ElementTypeId);

    }
}