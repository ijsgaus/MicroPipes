using System;
using MicroPipes.Schema.Green;

namespace MicroPipes.Schema
{
    public class WellKnownTypeSchema : TypeSchema
    {
        private readonly WellKnownTypeSchemaGreen _green;

        public WellKnownTypeSchema(WellKnownTypeSchemaGreen green)
        {
            _green = green;
        }

        public override string SchemaName => _green.SchemaName;

        public override string CodeName => _green.SchemaName;

        public override string ContractName => _green.SchemaName;

        public override Type DotNetType => _green.DotNetType.IfNoneDefault();

        public override bool IsWellKnown => true;
    }
}