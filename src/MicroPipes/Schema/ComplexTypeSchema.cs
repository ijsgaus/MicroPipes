using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MicroPipes.Schema.Green;

namespace MicroPipes.Schema
{
    public class ComplexTypeSchema : TypeSchema
    {
        private readonly ComplexTypeSchemaGreen _green;
        private Lazy<ImmutableDictionary<string, TypeSchema>> _lazyFields;
        public ServiceSchema Service { get; }

        public ComplexTypeSchema(ServiceSchema service, ComplexTypeSchemaGreen green)
        {
            _green = green;
            Service = service;
            _lazyFields = new Lazy<ImmutableDictionary<string, TypeSchema>>(
                () =>
                {
                    var builder = ImmutableDictionary.CreateBuilder<string, TypeSchema>();
                    builder.AddRange(_green.Fields.Select(p =>
                        new KeyValuePair<string, TypeSchema>(p.Key, Service.TypeById(p.Value))));
                    return builder.ToImmutable();
                });
        }

        public override string ContractName => _green.ContractName;


        public override string CodeName => _green.CodeName;

        public ServiceSchema SetCodeName(string value)
        {
            var newTypes = Service.Green.Types.SetItem(_green.Id, new ComplexTypeSchemaGreen(_green, _green.DotNetType,
                _green.SchemaName, value,
                _green.ContractName, _green.BaseTypeId, _green.IsStruct, _green.Fields));
            return new ServiceSchema(new ServiceSchemaGreen(Service.Green.Name, Service.Green.Owner, Service.Green.CodeName,
                Service.Green.Events, Service.Green.Calls, newTypes, Service.Green.ContentType, Service.Green.Exchange,
                Service.Green.ResponseExchange));
        }


        public override string SchemaName => _green.SchemaName;


        public override Type DotNetType => _green.DotNetType.IfNoneDefault();


        public override bool IsWellKnown => false;


        public bool IsStruct => _green.IsStruct;


        public ComplexTypeSchema BaseOn =>
             _green.BaseTypeId == null ? null : (ComplexTypeSchema) Service.TypeById(_green.BaseTypeId.Value);



        public IReadOnlyDictionary<string, TypeSchema> Fields => _lazyFields.Value;

    }
}