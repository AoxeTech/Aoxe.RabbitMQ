namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Send<T>(
        T? message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry) =>
        Send(GetTopicName(typeof(T)),
            message,
            persistence,
            publishRetry);

    /// <inheritdoc />
    public void Send<T>(
        string topic,
        T? message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry) =>
        Send(topic,
            _serializer.ToBytes(message),
            persistence,
            publishRetry);

    /// <inheritdoc />
    public void Send(
        string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry) =>
        Send(body,
            CreateExchangeParam(topic, persistence),
            CreateQueueParam(topic, persistence),
            null,
            null,
            persistence,
            publishRetry);
}