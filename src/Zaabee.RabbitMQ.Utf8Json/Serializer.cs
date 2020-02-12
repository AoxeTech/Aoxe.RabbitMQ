using System.Text;
using Utf8Json;
using Zaabee.RabbitMQ.ISerialize;
using Zaabee.Utf8Json;

namespace Zaabee.RabbitMQ.Utf8Json
{
    public class Serializer : ISerializer
    {
        private readonly IJsonFormatterResolver _defaultResolver;

        public Serializer(IJsonFormatterResolver defaultResolver = null)
        {
            _defaultResolver = defaultResolver;
        }

        public byte[] Serialize<T>(T t) => Utf8JsonSerializer.Serialize(t, _defaultResolver);

        public T Deserialize<T>(byte[] bytes) => Utf8JsonSerializer.Deserialize<T>(bytes, _defaultResolver);

        public string BytesToText(byte[] bytes) => Encoding.UTF8.GetString(bytes);

        public T FromText<T>(string text) => Utf8JsonSerializer.Deserialize<T>(text, _defaultResolver);
    }
}