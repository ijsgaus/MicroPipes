﻿using System;
using LanguageExt;

namespace MicroPipes.SchemaOld.Green
{
    public class EnumTypeSchemaGreen : TypeSchemaGreen
    {
        public EnumTypeSchemaGreen(Option<Type> dotNetType,
            string schemaName, string codeName, string contractName,
            int baseTypeId, bool isFlags, HashMap<string, long> values) : base(dotNetType, false)
        {
            SchemaName = schemaName;
            CodeName = codeName;
            ContractName = contractName;
            BaseTypeId = baseTypeId;
            IsFlags = isFlags;
            Values = values;
        }

        public EnumTypeSchemaGreen(TypeSchemaGreen @base, Option<Type> dotNetType, 
            string schemaName, string codeName, string contractName, int baseTypeId, bool isFlags, HashMap<string, long> values) : base(@base, dotNetType, false)
        {
            SchemaName = schemaName;
            CodeName = codeName;
            ContractName = contractName;
            BaseTypeId = baseTypeId;
            IsFlags = isFlags;
            Values = values;
        }

        public string SchemaName { get; }
        public string CodeName { get; }
        public string ContractName { get; }
        public int BaseTypeId { get; }
        public bool IsFlags { get; }
        public HashMap<string, long> Values { get; }  
    }
}