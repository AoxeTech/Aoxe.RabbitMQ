using System;
using System.Text;
using System.Text.Json;
using Zaabee.RabbitMQ.Serializer.Abstraction;
using Zaabee.SystemTextJson;

namespace Zaabee.RabbitMQ.SystemTextJson
{
    public class Serializer : ISerializer
    {
        private static Encoding _encoding;
        private static JsonSerializerOptions _jsonSerializerOptions;

        public Serializer(Encoding encoding = null, JsonSerializerOptions jsonSerializerOptions = null)
        {
            _encoding = encoding ?? Encoding.UTF8;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public ReadOnlyMemory<byte> Serialize<T>(T t) =>
            SystemTextJsonSerializer.Serialize(t, _jsonSerializerOptions);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) =>
            SystemTextJsonSerializer.Deserialize<T>(bytes.ToArray(), _jsonSerializerOptions);

        public string BytesToText(ReadOnlyMemory<byte> bytes) =>
            _encoding.GetString(bytes.ToArray());

        public T FromText<T>(string text) =>
            SystemTextJsonSerializer.Deserialize<T>(text, _jsonSerializerOptions);
    }
}