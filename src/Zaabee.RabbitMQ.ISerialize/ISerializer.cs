using System;

namespace Zaabee.RabbitMQ.ISerialize
{
    public interface ISerializer
    {
        ReadOnlyMemory<byte> Serialize<T>(T o);

        T Deserialize<T>(ReadOnlyMemory<byte> bytes);

        string BytesToText(ReadOnlyMemory<byte> bytes);

        T FromText<T>(string text);
    }
}