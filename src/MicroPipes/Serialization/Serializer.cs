using System;

namespace MicroPipes.Serialization
{

    public class Serializer : ISerializer
    {
        private Func<object, byte[]> _serializer;

        public Serializer(Func<object, byte[]> serializer)
        {
            _serializer = serializer;

        }

        public byte[] Serialize(object value) => _serializer(value);


    }
}