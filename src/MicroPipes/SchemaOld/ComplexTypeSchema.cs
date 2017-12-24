﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using MicroPipes.SchemaOld.Green;

namespace MicroPipes.SchemaOld
{
    public class ComplexTypeSchema : TypeSchema
    {
        private readonly ComplexTypeSchemaGreen _green;
        private Lazy<HashMap<string, TypeSchema>> _lazyFields;
        public ServiceSchema Service { get; }

        public ComplexTypeSchema(ServiceSchema service, ComplexTypeSchemaGreen green)
        {
            _green = green;
            Service = service;
            _lazyFields = new Lazy<HashMap<string, TypeSchema>>(
                    () => _green.Fields.Map(p => Service.TypeById(p)));
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


        public override Type DotNetType => _green.DotNetType.Unwrap();


        public override bool IsWellKnown => false;


        public bool IsStruct => _green.IsStruct;


        public ComplexTypeSchema BaseOn =>
             _green.BaseTypeId == null ? null : (ComplexTypeSchema) Service.TypeById(_green.BaseTypeId.Value);



        public IReadOnlyDictionary<string, TypeSchema> Fields => _lazyFields.Value.ToReadOnlyDictionary();

    }
}