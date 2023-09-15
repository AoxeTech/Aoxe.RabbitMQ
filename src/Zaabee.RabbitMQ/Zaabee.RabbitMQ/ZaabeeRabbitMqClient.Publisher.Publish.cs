namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Publish<T>(
        T? message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry) =>
        Publish(GetTopicName(typeof(T)),
            message,
            persistence,
            publishRetry);

    /// <inheritdoc />
    public void Publish<T>(
        string topic,
        T? message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry) =>
        Publish(topic,
            _serializer.ToBytes(message),
            persistence,
            publishRetry);

    /// <inheritdoc />
    public void Publish(
        string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry) =>
        Publish(body,
            GetExchangeParam(topic, persistence),
            persistence,
            publishRetry);
}