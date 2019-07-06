using Zaabee.RabbitMQ.ISerialize;
using Zaabee.Utf8Json;

namespace Zaabee.RabbitMQ.Utf8Json
{
    public class Serializer : ISerializer
    {
        public byte[] Serialize<T>(T o)
        {
            return o.Utf8JsonToBytes();
        }

        public byte[] StringToBytes(string str)
        {
            return str.SerializeUtf8();
        }

        public T Deserialize<T>(byte[] bytes)
        {
            return bytes.FromUtf8Json<T>();
        }

        public string ToString<T>(T o)
        {
            return o.Utf8JsonToString();
        }

        public string BytesToString(byte[] bytes)
        {
            return bytes.DeserializeUtf8();
        }

        public T FromString<T>(string str)
        {
            return str.FromUtf8Json<T>();
        }
    }
}