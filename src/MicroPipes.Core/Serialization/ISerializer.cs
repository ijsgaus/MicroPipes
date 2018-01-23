using System;

namespace MicroPipes.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize(object value);
        bool TryDeserialize(byte[] data, Type toType, out object value);
    }
}