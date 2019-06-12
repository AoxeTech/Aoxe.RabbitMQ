using System.Text;

namespace Zaabee.RabbitMQ.Utf8Json
{
    public static class EncodingExtension
    {
        public static byte[] SerializeUtf8(this string str)
        {
            return str != null ? Encoding.UTF8.GetBytes(str) : null;
        }

        public static string DeserializeUtf8(this byte[] bytes)
        {
            return bytes != null ? Encoding.UTF8.GetString(bytes) : null;
        }
    }
}