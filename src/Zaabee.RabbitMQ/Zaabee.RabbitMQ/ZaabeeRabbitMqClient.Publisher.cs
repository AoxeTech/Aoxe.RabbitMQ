namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private void Publish(
        byte[] body,
        ExchangeParam exchangeParam,
        QueueParam? queueParam,
        bool persistence,
        int retry = Consts.DefaultPublishRetry)
    {
        GetRetryPolicy(retry).Execute(() =>
        {
            IBasicProperties? properties = null;
            using var channel = GetPublisherChannel(exchangeParam, queueParam);
            if (persistence)
            {
                properties = channel.CreateBasicProperties();
                properties.Persistent = persistence;
            }
            channel.BasicPublish(exchangeParam.Exchange, DefaultRoutingKey, properties, body);
        });
    }

    private void Send(
        byte[] body,
        ExchangeParam normalExchangeParam,
        QueueParam? normalQueueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry)
    {
        GetRetryPolicy(publishRetry).Execute(() =>
        {
            IBasicProperties? properties = null;
            using var channel = GetPublisherChannel(normalExchangeParam, normalQueueParam, dlxExchangeParam, dlxQueueParam);
            if (persistence)
            {
                properties = channel.CreateBasicProperties();
                properties.Persistent = persistence;
            }
            channel.BasicPublish(normalExchangeParam.Exchange, DefaultRoutingKey, properties, body);
        });
    }

    private static Policy GetRetryPolicy(int retry) =>
        Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(retry, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, _) => throw ex);
}