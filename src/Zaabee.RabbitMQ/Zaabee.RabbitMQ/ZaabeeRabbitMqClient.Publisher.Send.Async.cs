namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public ValueTask SendAsync<T>(T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        CancellationToken cancellationToken = default)
    {
        Send(message, persistence, publishRetry, consumeRetry, dlx);
        return default;
    }

    /// <inheritdoc />
    public ValueTask SendAsync<T>(string topic,
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        CancellationToken cancellationToken = default)
    {
        Send(topic, message, persistence, publishRetry, consumeRetry, dlx);
        return default;
    }

    /// <inheritdoc />
    public ValueTask SendAsync(string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true,
        CancellationToken cancellationToken = default)
    {
        Send(topic, body, persistence, publishRetry, consumeRetry, dlx);
        return default;
    }
}