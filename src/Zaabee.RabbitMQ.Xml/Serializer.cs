using System.Text;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ.Xml
{
    public class Serializer : ISerializer
    {
        private static Encoding _encoding;

        public Serializer(Encoding defaultEncoding = null)
        {
            _encoding = defaultEncoding ?? Encoding.UTF8;
        }

        public byte[] Serialize<T>(T t) => Zaabee.Xml.XmlSerializer.Serialize(t);

        public T Deserialize<T>(byte[] bytes) => Zaabee.Xml.XmlSerializer.Deserialize<T>(bytes);

        public string BytesToText(byte[] bytes) => _encoding.GetString(bytes);

        public T FromText<T>(string text) => Zaabee.Xml.XmlSerializer.Deserialize<T>(text, _encoding);
    }
}