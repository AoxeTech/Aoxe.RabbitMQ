namespace Zaabee.RabbitMQ.ISerialize
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T o);

        byte[] StringToBytes(string str);

        T Deserialize<T>(byte[] bytes);

        string ToString<T>(T o);

        string BytesToString(byte[] bytes);

        T FromString<T>(string str);
    }
}