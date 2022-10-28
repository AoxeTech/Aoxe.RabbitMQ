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
        var exchangeParam = GetExchangeParam(topic, persistence);
        var queueParam = GetQueueParam(topic, persistence, SubscribeType.Receive);
        Publish(exchangeParam, queueParam, persistence, message);
    }

    public void SendMessage<T>(
        string topic,
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        var queueParam = GetQueueParam(topic, persistence, SubscribeType.Receive);
        Publish(exchangeParam, queueParam, persistence, message);
    }

    public void SendMessage(
        string topic,
        byte[] body,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        var queueParam = GetQueueParam(topic, persistence, SubscribeType.Receive);
        Publish(exchangeParam, queueParam, persistence, body);
    }
}