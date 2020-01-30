using System.Text;
using Jil;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.Jil
{
    public class Serializer : ISerializer
    {
        private static Encoding _encoding = Encoding.UTF8;

        public static Encoding DefaultEncoding
        {
            get => _encoding;
            set => _encoding = value ?? _encoding;
        }

        public static Options DefaultOptions;

        public Serializer(Encoding defaultEncoding = null, Options defaultOptions = null)
        {
            DefaultEncoding = defaultEncoding;
            DefaultOptions = defaultOptions;
        }

        public byte[] Serialize<T>(T t) =>
            t is null
                ? new byte[0]
                : DefaultEncoding.GetBytes(JSON.Serialize(t, DefaultOptions));

        public T Deserialize<T>(byte[] bytes) =>
            bytes is null || bytes.Length == 0
                ? default
                : JSON.Deserialize<T>(DefaultEncoding.GetString(bytes), DefaultOptions);

        public string BytesToText(byte[] bytes) =>
            bytes is null ? null : DefaultEncoding.GetString(bytes);

        public T FromText<T>(string text) =>
            string.IsNullOrWhiteSpace(text) ? default : JSON.Deserialize<T>(text, DefaultOptions);
    }
}