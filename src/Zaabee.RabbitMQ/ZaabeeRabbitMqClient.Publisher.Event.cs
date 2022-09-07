namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void PublishEvent<T>(T @event)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        Publish(exchangeParam, null, MessageType.Event, @event);
    }

    public void PublishEvent<T>(string topic, T @event)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        Publish(exchangeParam, null, MessageType.Event, @event);
    }

    public void PublishEvent(string topic, byte[] body)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        Publish(exchangeParam, null, MessageType.Event, body);
    }
}