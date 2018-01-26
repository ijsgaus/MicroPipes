using System.Net.Mime;

namespace MicroPipes
{
    public class SendContext
    {
        public string Requester { get; }
        public ContentType ContentType { get; }
    }
}