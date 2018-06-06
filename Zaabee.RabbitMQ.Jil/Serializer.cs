using Zaabee.Jil;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.Jil
{
    public class Serializer : ISerializer
    {
        public byte[] Serialize<T>(T o)
        {
            return o == null ? new byte[0] : o.ToJil().SerializeUtf8();
        }

        public T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return default(T);
            return bytes.DeserializeUtf8().FromJil<T>();
        }

        public string ToString<T>(T o)
        {
            return o == null ? null : o.ToJil();
        }

        public T FromString<T>(string json)
        {
            return string.IsNullOrWhiteSpace(json) ? default(T) : json.FromJil<T>();
        }
    }
}