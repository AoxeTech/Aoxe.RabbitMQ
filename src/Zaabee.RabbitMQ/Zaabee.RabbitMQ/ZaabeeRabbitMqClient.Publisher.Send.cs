namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Send<T>(
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry)
    {
        var topic = GetTypeName(typeof(T));
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Send(_serializer.ToBytes(message),
            normalExchangeParam,
            normalQueueParam,
            persistence,
            publishRetry);
    }

    /// <inheritdoc />
    public void Send<T>(string topic,
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry)
    {
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Send(_serializer.ToBytes(message),
            normalExchangeParam,
            normalQueueParam,
            persistence,
            publishRetry);
    }

    /// <inheritdoc />
    public void Send(string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry)
    {
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        Send(body,
            normalExchangeParam,
            normalQueueParam,
            persistence,
            publishRetry);
    }
}