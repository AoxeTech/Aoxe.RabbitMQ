using System.Text;
using System.Text.Json;
using Zaabee.RabbitMQ.ISerialize;
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

        public byte[] Serialize<T>(T t) => SystemTextJsonSerializer.Serialize(t, _jsonSerializerOptions);

        public T Deserialize<T>(byte[] bytes) => SystemTextJsonSerializer.Deserialize<T>(bytes, _jsonSerializerOptions);

        public string BytesToText(byte[] bytes) => Encoding.GetString(bytes);

        public T FromText<T>(string text) => SystemTextJsonSerializer.Deserialize<T>(text, _jsonSerializerOptions);
    }
}