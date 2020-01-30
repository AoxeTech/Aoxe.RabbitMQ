using System.Text;
using System.Text.Json;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.SystemTextJson
{
    public class Serializer : ISerializer
    {
        private static Encoding _encoding = Encoding.UTF8;

        public static Encoding DefaultEncoding
        {
            get => _encoding;
            set => _encoding = value ?? _encoding;
        }

        public static JsonSerializerOptions DefaultJsonSerializerOptions;

        public Serializer(Encoding defaultEncoding = null, JsonSerializerOptions defaultJsonSerializerOptions = null)
        {
            DefaultEncoding = defaultEncoding;
            DefaultJsonSerializerOptions = defaultJsonSerializerOptions;
        }

        public byte[] Serialize<T>(T t) =>
            t is null
                ? new byte[0]
                : DefaultEncoding.GetBytes(JsonSerializer.Serialize(t, DefaultJsonSerializerOptions));

        public T Deserialize<T>(byte[] bytes) =>
            bytes is null || bytes.Length == 0
                ? default
                : JsonSerializer.Deserialize<T>(DefaultEncoding.GetString(bytes), DefaultJsonSerializerOptions);

        public string BytesToText(byte[] bytes) =>
            bytes is null ? null : DefaultEncoding.GetString(bytes);

        public T FromText<T>(string text) =>
            string.IsNullOrWhiteSpace(text)
                ? default
                : JsonSerializer.Deserialize<T>(text, DefaultJsonSerializerOptions);
    }
}