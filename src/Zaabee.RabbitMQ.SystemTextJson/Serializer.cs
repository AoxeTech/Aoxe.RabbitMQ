using System;
using System.Text;
using System.Text.Json;
using Zaabee.RabbitMQ.Serializer.Abstraction;
using Zaabee.SystemTextJson;

namespace Zaabee.RabbitMQ.SystemTextJson
{
    public class Serializer : ISerializer
    {
        private static JsonSerializerOptions _jsonSerializerOptions;

        public Serializer(JsonSerializerOptions jsonSerializerOptions = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public ReadOnlyMemory<byte> Serialize<T>(T t) => t.ToBytes(_jsonSerializerOptions);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) => bytes.ToArray().FromBytes<T>(_jsonSerializerOptions);

        public string BytesToText(ReadOnlyMemory<byte> bytes) => Encoding.UTF8.GetString(bytes.ToArray());

        public T FromText<T>(string text) => text.FromJson<T>(_jsonSerializerOptions);
    }
}