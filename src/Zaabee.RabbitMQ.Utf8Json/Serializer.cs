using System;
using System.Text;
using Utf8Json;
using Zaabee.RabbitMQ.Serializer.Abstraction;
using Zaabee.Utf8Json;

namespace Zaabee.RabbitMQ.Utf8Json
{
    public class Serializer : ISerializer
    {
        private readonly IJsonFormatterResolver _jsonFormatterResolver;

        public Serializer(IJsonFormatterResolver jsonFormatterResolver = null)
        {
            _jsonFormatterResolver = jsonFormatterResolver;
        }

        public ReadOnlyMemory<byte> Serialize<T>(T t) =>
            Utf8JsonSerializer.Serialize(t, _jsonFormatterResolver);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) =>
            Utf8JsonSerializer.Deserialize<T>(bytes.ToArray(), _jsonFormatterResolver);

        public string BytesToText(ReadOnlyMemory<byte> bytes) =>
            Encoding.UTF8.GetString(bytes.ToArray());

        public T FromText<T>(string text) =>
            Utf8JsonSerializer.Deserialize<T>(text, _jsonFormatterResolver);
    }
}