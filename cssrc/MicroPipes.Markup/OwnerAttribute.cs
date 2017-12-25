using System;

namespace MicroPipes.Markup
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class OwnerAttribute : Attribute
    {
        public string OwnerName { get; }

        public OwnerAttribute(string ownerName)
        {
            OwnerName = ownerName;
        }
        
    }
}