namespace Zaabee.RabbitMQ.ISerialize
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T o);

        T Deserialize<T>(byte[] bytes);

        string BytesToText(byte[] bytes);

        T FromText<T>(string text);
    }
}