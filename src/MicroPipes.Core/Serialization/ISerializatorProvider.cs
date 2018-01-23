using System.Net.Mime;

namespace MicroPipes.Serialization
{
    public interface ISerializatorProvider
    {
        ISerializer GetSerializer(ContentType contentType, string defaultEncoding);
        IDeserialize GetDeserializer(ContentType contentType, string defaultEncoding);
    }
}