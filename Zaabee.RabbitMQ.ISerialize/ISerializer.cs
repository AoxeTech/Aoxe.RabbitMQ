namespace Zaabee.RabbitMQ.ISerialize
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T o);

        T Deserialize<T>(byte[] bytes);

        string ToString<T>(T o);

        T FromString<T>(string json);
    }
}