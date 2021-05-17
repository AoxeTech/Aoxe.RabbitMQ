using System;
using System.Text;
using Jil;
using Zaabee.Jil;
using Zaabee.RabbitMQ.Serializer.Abstraction;

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

        public ReadOnlyMemory<byte> Serialize<T>(T t) => t.ToBytes(_options, _encoding);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) => bytes.ToArray().FromBytes<T>(_options, _encoding);

        public string BytesToText(ReadOnlyMemory<byte> bytes) => _encoding.GetString(bytes.ToArray());

        public T FromText<T>(string text) => text.FromJson<T>(_options);
    }
}