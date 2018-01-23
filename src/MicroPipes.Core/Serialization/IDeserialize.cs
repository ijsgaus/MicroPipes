using System;

namespace MicroPipes.Serialization
{
    public interface IDeserialize 
    {
        bool TryDeserialize(byte[] data, Type toType, out object value);
    }
}