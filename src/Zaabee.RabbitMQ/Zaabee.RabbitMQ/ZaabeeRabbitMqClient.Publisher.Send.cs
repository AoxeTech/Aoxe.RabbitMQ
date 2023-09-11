namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void Send<T>(
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, persistence);
        var queueParam = GetQueueParam(topic, persistence, SubscribeType.Receive);
        Publish(message, exchangeParam, queueParam, persistence);
    }

    public void Send<T>(
        string topic,
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        var queueParam = GetQueueParam(topic, persistence, SubscribeType.Receive);
        Publish(message, exchangeParam, queueParam, persistence);
    }

    public void Send(
        string topic,
        byte[] body,
        bool persistence = false,
        int retry = 3,
        bool dlx = true)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        var queueParam = GetQueueParam(topic, persistence, SubscribeType.Receive);
        Publish(body, exchangeParam, queueParam, persistence);
    }
}