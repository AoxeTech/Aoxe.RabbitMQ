using Zaabee.Json;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.Json
{
    public class Serializer : ISerializer
    {
        public byte[] Serialize<T>(T o)
        {
            return o == null ? new byte[0] : o.ToJson().SerializeUtf8();
        }

        public T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return default(T);
            return bytes.DeserializeUtf8().FromJson<T>();
        }

        public string ToString<T>(T o)
        {
            return o == null ? null : o.ToJson();
        }

        public T FromString<T>(string json)
        {
            return string.IsNullOrWhiteSpace(json) ? default(T) : json.FromJson<T>();
        }
    }
}