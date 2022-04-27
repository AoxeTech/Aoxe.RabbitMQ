namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void SendEvent<T>(T @event)
    {
        var topic = GetTypeName(typeof(T));
        Publish(topic, topic, MessageType.Event, _serializer.ToBytes(@event));
    }

    public void SendEvent(string topic, byte[] body) =>
        Publish(topic, topic, MessageType.Event, body);

    public void PublishEvent<T>(T @event) =>
        Publish(GetTypeName(typeof(T)), null, MessageType.Event, _serializer.ToBytes(@event));

    public void PublishEvent<T>(string topic, T @event) =>
        Publish(topic, null, MessageType.Event, _serializer.ToBytes(@event));

    public void PublishEvent(string topic, byte[] body) =>
        Publish(topic, null, MessageType.Event, body);
}