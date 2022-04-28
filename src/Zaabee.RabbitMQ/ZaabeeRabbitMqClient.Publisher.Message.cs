namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void SendMessage<T>(T message)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message);
        Publish(exchangeParam, queueParam, MessageType.Message, message);
    }

    public void SendMessage(string topic, byte[] body)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message);
        Publish(exchangeParam, queueParam, MessageType.Message, body);
    }

    public void PublishMessage<T>(T message)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        Publish(exchangeParam, null, MessageType.Message, message);
    }

    public void PublishMessage<T>(string topic, T message)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        Publish(exchangeParam, null, MessageType.Message, message);
    }

    public void PublishMessage(string topic, byte[] body)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        Publish(exchangeParam, null, MessageType.Message, body);
    }
}