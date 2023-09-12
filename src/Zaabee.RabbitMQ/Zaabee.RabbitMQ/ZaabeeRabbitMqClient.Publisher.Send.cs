namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    /// <inheritdoc />
    public void Send<T>(
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true)
    {
        var topic = GetTypeName(typeof(T));
        var (normalExchangeParam,
                normalQueueParam,
                dlxExchangeParam,
                dlxQueueParam) =
            GenerateSendParams(topic, persistence, consumeRetry, dlx);
        Send(_serializer.ToBytes(message),
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            persistence,
            publishRetry);
    }

    /// <inheritdoc />
    public void Send<T>(string topic,
        T message,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true)
    {
        var (normalExchangeParam,
                normalQueueParam,
                dlxExchangeParam,
                dlxQueueParam) =
            GenerateSendParams(topic, persistence, consumeRetry, dlx);
        Send(_serializer.ToBytes(message),
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            persistence,
            publishRetry);
    }

    /// <inheritdoc />
    public void Send(string topic,
        byte[] body,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry,
        int consumeRetry = Consts.DefaultConsumeRetry,
        bool dlx = true)
    {
        var (normalExchangeParam,
                normalQueueParam,
                dlxExchangeParam,
                dlxQueueParam) =
            GenerateSendParams(topic, persistence, consumeRetry, dlx);
        Send(body,
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam,
            dlxQueueParam,
            persistence,
            publishRetry);
    }

    private (ExchangeParam normalExchangeParam,
        QueueParam normalQueueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam) GenerateSendParams(
            string topic,
            bool persistence,
            int consumeRetry = Consts.DefaultConsumeRetry,
            bool dlx = true)
    {
        var normalExchangeParam = GetExchangeParam(topic, persistence);
        var normalQueueParam = GetQueueParam(topic, persistence, SubscribeType.Receive);
        ExchangeParam? dlxExchangeParam = null;
        QueueParam? dlxQueueParam = null;

        if (!dlx) return (normalExchangeParam, normalQueueParam, dlxExchangeParam, dlxQueueParam);

        dlxExchangeParam = GetExchangeParam($"{topic}[dlx]", persistence);
        dlxQueueParam = GetQueueParam($"{topic}[dlx]", persistence, SubscribeType.Receive);
        return (normalExchangeParam, normalQueueParam, dlxExchangeParam, dlxQueueParam);
    }
}