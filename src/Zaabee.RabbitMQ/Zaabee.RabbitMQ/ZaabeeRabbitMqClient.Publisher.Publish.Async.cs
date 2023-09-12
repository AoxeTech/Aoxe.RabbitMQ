namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public ValueTask PublishAsync<T>(
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default)
    {
        Publish(message, persistence, publishRetry);
        return default;
    }

    /// <inheritdoc />
    public ValueTask PublishAsync<T>(
        string topic,
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default)
    {
        Publish(topic, message, persistence, publishRetry);
        return default;
    }

    /// <inheritdoc />
    public ValueTask PublishAsync(
        string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        CancellationToken cancellationToken = default)
    {
        Publish(topic, body, persistence, publishRetry);
        return default;
    }
}