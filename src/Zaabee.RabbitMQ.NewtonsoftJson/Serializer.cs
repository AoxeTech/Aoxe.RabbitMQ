using System.Text;
using Newtonsoft.Json;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.NewtonsoftJson
{
    public class Serializer : ISerializer
    {
        private static Encoding _encoding = Encoding.UTF8;

        public static Encoding DefaultEncoding
        {
            get => _encoding;
            set => _encoding = value ?? _encoding;
        }

        public static JsonSerializerSettings DefaultSettings;

        public Serializer(Encoding defaultEncoding = null, JsonSerializerSettings defaultSettings = null)
        {
            DefaultEncoding = defaultEncoding;
            DefaultSettings = defaultSettings;
        }

        public byte[] Serialize<T>(T t) =>
            t is null
                ? new byte[0]
                : DefaultEncoding.GetBytes(JsonConvert.SerializeObject(t, DefaultSettings));

        public T Deserialize<T>(byte[] bytes) =>
            bytes is null || bytes.Length == 0
                ? default
                : JsonConvert.DeserializeObject<T>(DefaultEncoding.GetString(bytes), DefaultSettings);

        public string BytesToText(byte[] bytes) =>
            bytes is null ? null : DefaultEncoding.GetString(bytes);

        public T FromText<T>(string text) =>
            string.IsNullOrWhiteSpace(text) ? default : JsonConvert.DeserializeObject<T>(text, DefaultSettings);
    }
}