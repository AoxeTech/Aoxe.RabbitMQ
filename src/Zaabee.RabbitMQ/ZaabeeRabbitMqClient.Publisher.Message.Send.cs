namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void SendMessage<T>(
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message);
        Publish(exchangeParam, queueParam, MessageType.Message, message);
    }

    public void SendMessage<T>(
        string topic,
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message);
        Publish(exchangeParam, queueParam, MessageType.Message, message);
    }

    public void SendMessage(
        string topic,
        byte[] body,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Message);
        var queueParam = GetQueueParam(topic, MessageType.Message);
        Publish(exchangeParam, queueParam, MessageType.Message, body);
    }
}