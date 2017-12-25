using System;
using System.Collections.Generic;

using System.Linq;
using LanguageExt;

namespace MicroPipes.SchemaOld.Green
{
    public class WellKnownTypeSchemaGreen : TypeSchemaGreen, IHasSchemaName
    {
        private WellKnownTypeSchemaGreen(string name, Type dotNetType) : base(dotNetType, true)
        {
            if (dotNetType == null) throw new ArgumentNullException(nameof(dotNetType));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            SchemaName = name;
        }

        public string SchemaName { get; }


        
        

        static WellKnownTypeSchemaGreen()
        {
            var lst = new[]
            {
                new WellKnownTypeSchemaGreen("u8", typeof(byte)),
                new WellKnownTypeSchemaGreen("i8", typeof(sbyte)),
                new WellKnownTypeSchemaGreen("u16", typeof(ushort)),
                new WellKnownTypeSchemaGreen("i16", typeof(short)),
                new WellKnownTypeSchemaGreen("u32", typeof(uint)),
                new WellKnownTypeSchemaGreen("i32", typeof(int)),
                new WellKnownTypeSchemaGreen("u64", typeof(ulong)),
                new WellKnownTypeSchemaGreen("i64", typeof(long)),
                new WellKnownTypeSchemaGreen("f32", typeof(float)),
                new WellKnownTypeSchemaGreen("f64", typeof(double)),
                new WellKnownTypeSchemaGreen("string", typeof(string)),
                new WellKnownTypeSchemaGreen("uuid", typeof(Guid)),      
                new WellKnownTypeSchemaGreen("datetime", typeof(DateTime)),
                new WellKnownTypeSchemaGreen("datetime2", typeof(DateTimeOffset)),
                new WellKnownTypeSchemaGreen("timespan", typeof(TimeSpan)),
                new WellKnownTypeSchemaGreen("fail", typeof(RequestProcessError)),
                new WellKnownTypeSchemaGreen("unit", typeof(ValueTuple)) 
            };
            
            ByCode = HashMap.createRange(lst.Select(p => (p.SchemaName, p)));
            
            ByType = HashMap.createRange(lst.Select(p => (p.DotNetType.Unwrap(), p)));
        }

        public static HashMap<string, WellKnownTypeSchemaGreen> ByCode { get; }
        public static HashMap<Type, WellKnownTypeSchemaGreen> ByType { get; }

    }
}