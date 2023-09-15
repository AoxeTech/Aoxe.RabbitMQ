namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Send<T>(
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        bool dlx = true) =>
        Send(GetTopicName(typeof(T)),
            message,
            persistence,
            publishRetry,
            dlx);

    /// <inheritdoc />
    public void Send<T>(
        string topic,
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        bool dlx = true) =>
        Send(topic,
            _serializer.ToBytes(message),
            persistence,
            publishRetry,
            dlx);

    /// <inheritdoc />
    public void Send(
        string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        bool dlx = true)
    {
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence);
        var dlxExchangeParam = dlx ? GetExchangeParam(topic, persistence, ExchangeRole.Dlx) : null;
        var dlxQueueParam = dlx ? GetQueueParam(topic, persistence, false, QueueRole.Dlx) : null;
        Send(body,
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            persistence,
            publishRetry);
    }
}