using System;

namespace MicroPipes.Serialization
{
    public class Deserializer : IDeserialize

    {
        private readonly Func<byte[], Type, (bool, object)> _deserializer;

        public Deserializer(Func<byte[], Type, (bool, object)> deserializer)
        {
            _deserializer = deserializer;
        }

        public bool TryDeserialize(byte[] data, Type toType, out object value)
        {
            var (success, val) = _deserializer(data, toType);
            value = val;
            return success;
        }
    }
}