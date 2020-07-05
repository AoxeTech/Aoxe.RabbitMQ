using System;
using System.Text;
using Zaabee.RabbitMQ.Serializer.Abstraction;

namespace Zaabee.RabbitMQ.Xml
{
    public class Serializer : ISerializer
    {
        private static Encoding _encoding;

        public Serializer(Encoding defaultEncoding = null)
        {
            _encoding = defaultEncoding ?? Encoding.UTF8;
        }

        public ReadOnlyMemory<byte> Serialize<T>(T t) =>
            Zaabee.Xml.XmlSerializer.Serialize(t);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) =>
            Zaabee.Xml.XmlSerializer.Deserialize<T>(bytes.ToArray());

        public string BytesToText(ReadOnlyMemory<byte> bytes) =>
            _encoding.GetString(bytes.ToArray());

        public T FromText<T>(string text) =>
            Zaabee.Xml.XmlSerializer.Deserialize<T>(text, _encoding);
    }
}