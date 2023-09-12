namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Publish<T>(
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(_serializer.ToBytes(message), exchangeParam, null, persistence, publishRetry);
    }

    /// <inheritdoc />
    public void Publish<T>(
        string topic,
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(_serializer.ToBytes(message), exchangeParam, null, persistence, publishRetry);
    }

    /// <inheritdoc />
    public void Publish(
        string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(body, exchangeParam, null, persistence, publishRetry);
    }
}