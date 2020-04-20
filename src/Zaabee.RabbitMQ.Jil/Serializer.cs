using System;
using System.Text;
using Jil;
using Zaabee.Jil;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.Jil
{
    public class Serializer : ISerializer
    {
        private static Encoding _encoding;
        private static Options _options;

        public Serializer(Encoding encoding = null, Options options = null)
        {
            _encoding = encoding ?? Encoding.UTF8;
            _options = options;
        }

        public ReadOnlyMemory<byte> Serialize<T>(T t) =>
            JilSerializer.Serialize(t, _options, _encoding);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) =>
            JilSerializer.Deserialize<T>(bytes.ToArray(), _options, _encoding);

        public string BytesToText(ReadOnlyMemory<byte> bytes) =>
            _encoding.GetString(bytes.ToArray());

        public T FromText<T>(string text) =>
            JilSerializer.Deserialize<T>(text, _options);
    }
}