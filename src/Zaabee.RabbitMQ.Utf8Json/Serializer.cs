using System;
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

        public ReadOnlyMemory<byte> Serialize<T>(T t) =>
            Utf8JsonSerializer.Serialize(t, _defaultResolver);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) =>
            Utf8JsonSerializer.Deserialize<T>(bytes.ToArray(), _defaultResolver);

        public string BytesToText(ReadOnlyMemory<byte> bytes) =>
            Encoding.UTF8.GetString(bytes.ToArray());

        public T FromText<T>(string text) =>
            Utf8JsonSerializer.Deserialize<T>(text, _defaultResolver);
    }
}