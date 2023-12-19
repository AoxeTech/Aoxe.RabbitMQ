namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Publish<T>(
        string topic,
        T? message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        string? queueName = null
    ) => Publish(topic, _serializer.ToBytes(message), persistence, publishRetry, queueName);

    /// <inheritdoc />
    public void Publish(
        string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        string? queueName = null
    ) =>
        Publish(
            body,
            CreateExchangeParam(topic, persistence),
            queueName is null ? null : CreateQueueParam(queueName, persistence),
            persistence,
            publishRetry
        );
}
