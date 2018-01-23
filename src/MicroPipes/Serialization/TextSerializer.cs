using System;
using System.Net.Mime;
using System.Text;
using JetBrains.Annotations;

namespace MicroPipes.Serialization
{
    public abstract class TextSerializerProvider : ISerializerProvider 
    {
        public ISerializer GetSerializer([NotNull] ContentType contentType, [NotNull] string defaultEncoding)
        {
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(defaultEncoding))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(defaultEncoding));
            if (!IsMediaTypeSupported(contentType.MediaType))
                return null;
            var encoding = Encoding.GetEncoding(
                string.IsNullOrWhiteSpace(contentType.CharSet) ? defaultEncoding : contentType.CharSet);
            return new Serializer(v => encoding.GetBytes(Serialize(contentType.MediaType, v)),
                (body, type) => Deserialize(contentType.MediaType, encoding.GetString(body), type));
        }

        protected abstract bool IsMediaTypeSupported(string mediaType);

        protected abstract string Serialize(string mediaType, object value);
        protected abstract (bool, object) Deserialize(string mediaType, string data, Type toType);

        
        
        private class Serializer : ISerializer
        {
            private Func<object, byte[]> _serializer;
            private Func<byte[], Type, (bool, object)> _deserializer;

            public Serializer(Func<object, byte[]> serializer, Func<byte[], Type, (bool, object)> deserializer)
            {
                _serializer = serializer;
                _deserializer = deserializer;
            }

            public byte[] Serialize(object value) => _serializer(value);


            public bool TryDeserialize(byte[] data, Type toType, out object value)
            {
                var (success, val) = _deserializer(data, toType);
                value = val;
                return success;
            }
        }
    }
}