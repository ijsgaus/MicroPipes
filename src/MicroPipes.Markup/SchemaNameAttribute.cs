using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace MicroPipes.Markup
{
    [AttributeUsage(AttributeTargets.All)]
    public class SchemaNameAttribute : Attribute
    {
        private static readonly Regex QualifiedName = new Regex(@"(([A-Z][a-z0-9]*)\.)*[A-Z][a-z0-9]*$");  
        
        public SchemaNameAttribute(string name)
        {
            if(!QualifiedName.Match(name).Success)
                throw new ArgumentException("Name must be quaified name");
            Name = name;
        }

        public string Name { get; }
        
        public string AttachToNamespace { get; set; }
    }
}