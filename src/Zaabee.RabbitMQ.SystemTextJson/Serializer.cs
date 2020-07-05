using System;
using System.Text;
using System.Text.Json;
using Zaabee.RabbitMQ.Serializer.Abstraction;
using Zaabee.SystemTextJson;

namespace Zaabee.RabbitMQ.SystemTextJson
{
    public class Serializer : ISerializer
    {
        private static readonly Encoding Encoding = Encoding.UTF8;
        private static JsonSerializerOptions _jsonSerializerOptions;

        public Serializer(JsonSerializerOptions jsonSerializerOptions = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public ReadOnlyMemory<byte> Serialize<T>(T t) =>
            SystemTextJsonSerializer.Serialize(t, _jsonSerializerOptions);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) =>
            SystemTextJsonSerializer.Deserialize<T>(bytes.ToArray(), _jsonSerializerOptions);

        public string BytesToText(ReadOnlyMemory<byte> bytes) =>
            Encoding.GetString(bytes.ToArray());

        public T FromText<T>(string text) =>
            SystemTextJsonSerializer.Deserialize<T>(text, _jsonSerializerOptions);
    }
}