using System.Net.Mime;

namespace MicroPipes.Serialization
{
    public interface ISerializerProvider
    {
        ISerializer GetSerializer(ContentType contentType, string defaultEncoding);
    }
}