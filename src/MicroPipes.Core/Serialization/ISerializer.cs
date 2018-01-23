namespace MicroPipes.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize(object value);
    }
}