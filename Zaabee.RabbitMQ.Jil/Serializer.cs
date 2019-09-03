using System.Text;
using Zaabee.Jil;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.Jil
{
    public class Serializer : ISerializer
    {
        public byte[] Serialize<T>(T o) =>
            o.ToBytes();

        public T Deserialize<T>(byte[] bytes) =>
            bytes is null || bytes.Length == 0 ? default(T) : bytes.FromBytes<T>();

        public string BytesToText(byte[] bytes) =>
            bytes != null ? Encoding.UTF8.GetString(bytes) : null;

        public T FromText<T>(string bytesToText) =>
            string.IsNullOrWhiteSpace(bytesToText) ? default(T) : bytesToText.FromJson<T>();
    }
}