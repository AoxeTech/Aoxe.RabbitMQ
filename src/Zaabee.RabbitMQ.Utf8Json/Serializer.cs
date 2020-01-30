using System.Text;
using Utf8Json;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.Utf8Json
{
    public class Serializer : ISerializer
    {
        public IJsonFormatterResolver DefaultResolver { get; set; }

        public Serializer(IJsonFormatterResolver defaultResolver = null)
        {
            DefaultResolver = defaultResolver;
        }

        public byte[] Serialize<T>(T t) =>
            JsonSerializer.Serialize(t, DefaultResolver);

        public T Deserialize<T>(byte[] bytes) =>
            bytes is null || bytes.Length == 0 ? default : JsonSerializer.Deserialize<T>(bytes, DefaultResolver);

        public string BytesToText(byte[] bytes) =>
            bytes is null ? null : Encoding.UTF8.GetString(bytes);

        public T FromText<T>(string text) =>
            string.IsNullOrWhiteSpace(text) ? default : JsonSerializer.Deserialize<T>(text, DefaultResolver);
    }
}