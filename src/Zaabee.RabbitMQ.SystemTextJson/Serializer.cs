using System.Text;
using Zaabee.RabbitMQ.ISerialize;
using Zaabee.SystemTextJson;

namespace Zaabee.RabbitMQ.SystemTextJson
{
    public class Serializer: ISerializer
    {
        public byte[] Serialize<T>(T o) =>
            o.ToBytes();

        public T Deserialize<T>(byte[] bytes) =>
            bytes is null || bytes.Length == 0 ? default : bytes.FromBytes<T>();

        public string BytesToText(byte[] bytes) =>
            bytes != null ? Encoding.UTF8.GetString(bytes) : null;

        public T FromText<T>(string text) =>
            string.IsNullOrWhiteSpace(text) ? default : text.FromJson<T>();
    }
}