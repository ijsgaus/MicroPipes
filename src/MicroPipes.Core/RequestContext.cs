using System.Net.Mime;

namespace MicroPipes
{
    public class RequestContext
    {
        public string Requester { get; }
        public ContentType ContentType { get; }
    }
}