namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void SendMessage<T>(T message)
    {
        var topic = GetTypeName(typeof(T));
        Publish(topic, topic, MessageType.Message, _serializer.ToBytes(message));
    }

    public void SendMessage(string topic, byte[] body) =>
        Publish(topic, topic, MessageType.Message, body);

    public void PublishMessage<T>(T message) =>
        Publish(GetTypeName(typeof(T)), null, MessageType.Message, _serializer.ToBytes(message));

    public void PublishMessage<T>(string topic, T message) =>
        Publish(topic, null, MessageType.Message, _serializer.ToBytes(message));

    public void PublishMessage(string topic, byte[] body) =>
        Publish(topic, null, MessageType.Message, body);
}