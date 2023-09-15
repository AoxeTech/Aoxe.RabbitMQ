namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Send<T>(
        T? message,
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
        T? message,
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
        bool dlx = true) =>
        Send(body,
            GetExchangeParam(topic, persistence),
            GetQueueParam(topic, persistence),
            dlx ? GetExchangeParam(topic, persistence, ExchangeRole.Dlx) : null,
            dlx ? GetQueueParam(topic, persistence, false, QueueRole.Dlx) : null,
            persistence,
            publishRetry);
}