using System.Text;
using Newtonsoft.Json;
using Zaabee.NewtonsoftJson;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.NewtonsoftJson
{
    public class Serializer : ISerializer
    {
        private static Encoding _encoding;
        private static JsonSerializerSettings _settings;

        public Serializer(Encoding encoding = null, JsonSerializerSettings settings = null)
        {
            _encoding = encoding ?? Encoding.UTF8;
            _settings = settings;
        }

        public byte[] Serialize<T>(T t) => NewtonsoftJsonSerializer.Serialize(t, _settings, _encoding);

        public T Deserialize<T>(byte[] bytes) => NewtonsoftJsonSerializer.Deserialize<T>(bytes, _settings, _encoding);

        public string BytesToText(byte[] bytes) => _encoding.GetString(bytes);

        public T FromText<T>(string text) => NewtonsoftJsonSerializer.Deserialize<T>(text, _settings);
    }
}