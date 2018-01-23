using System;
using System.Net.Mime;
using System.Text;
using JetBrains.Annotations;

namespace MicroPipes.Serialization
{
    public abstract class TextSerializatorProvider : ISerializatorProvider 
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
            return new Serializer(v => encoding.GetBytes(Serialize(contentType.MediaType, v)));
        }

        public IDeserialize GetDeserializer(ContentType contentType, string defaultEncoding)
        {
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(defaultEncoding))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(defaultEncoding));
            if (!IsMediaTypeSupported(contentType.MediaType))
                return null;
            var encoding = Encoding.GetEncoding(
                string.IsNullOrWhiteSpace(contentType.CharSet) ? defaultEncoding : contentType.CharSet);
            return new Deserializer(
                (body, type) => Deserialize(contentType.MediaType, encoding.GetString(body), type));
        }

        protected abstract bool IsMediaTypeSupported(string mediaType);

        protected abstract string Serialize(string mediaType, object value);
        protected abstract (bool, object) Deserialize(string mediaType, string data, Type toType);
    }
}