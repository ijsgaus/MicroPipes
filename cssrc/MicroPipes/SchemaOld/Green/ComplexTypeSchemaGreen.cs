using System;
using LanguageExt;

namespace MicroPipes.SchemaOld.Green
{
    public class ComplexTypeSchemaGreen : TypeSchemaGreen
    {
        public ComplexTypeSchemaGreen(Option<Type> dotNetType, string schemaName, string codeName, string contractName,
            int? baseTypeId, bool isStruct, HashMap<string, int> fields) : base(dotNetType, false)
        {
            SchemaName = schemaName;
            CodeName = codeName;
            ContractName = contractName;
            BaseTypeId = baseTypeId;
            IsStruct = isStruct;
            Fields = fields;
        }

        public ComplexTypeSchemaGreen(TypeSchemaGreen @base, Option<Type> dotNetType, string schemaName,
            string codeName, string contractName, int? baseTypeId, bool isStruct,
            HashMap<string, int> fields) : base(@base, dotNetType, false)
        {
            SchemaName = schemaName;
            CodeName = codeName;
            ContractName = contractName;
            BaseTypeId = baseTypeId;
            IsStruct = isStruct;
            Fields = fields;
        }

        public string SchemaName { get; }
        public string CodeName { get; }
        public string ContractName { get; }
        public int? BaseTypeId { get; }
        public bool IsStruct { get; }
        public HashMap<string, int> Fields { get; }
    }
}