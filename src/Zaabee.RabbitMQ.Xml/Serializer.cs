using System;
using System.Text;
using Zaabee.RabbitMQ.Serializer.Abstraction;
using Zaabee.Xml;

namespace Zaabee.RabbitMQ.Xml
{
    public class Serializer : ISerializer
    {
        public ReadOnlyMemory<byte> Serialize<T>(T t) =>
            XmlSerializer.Serialize(t);

        public T Deserialize<T>(ReadOnlyMemory<byte> bytes) =>
            XmlSerializer.Deserialize<T>(bytes.ToArray());

        public string BytesToText(ReadOnlyMemory<byte> bytes) =>
            Encoding.UTF8.GetString(bytes.ToArray());

        public T FromText<T>(string text) =>
            XmlSerializer.Deserialize<T>(text);
    }
}