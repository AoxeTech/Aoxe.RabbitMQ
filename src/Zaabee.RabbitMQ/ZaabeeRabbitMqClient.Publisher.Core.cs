namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private void Publish(
        byte[] body,
        ExchangeParam normalExchangeParam,
        QueueParam? normalQueueParam,
        bool persistence,
        int publishRetry = Consts.DefaultPublishRetry
    ) =>
        GetRetryPolicy(publishRetry)
            .Execute(() =>
            {
                IBasicProperties? properties = null;
                using var channel = GenerateChannel(
                    _publishConn,
                    normalExchangeParam,
                    normalQueueParam,
                    retryExchangeParam: null,
                    dlxExchangeParam: null,
                    dlxQueueParam: null
                );
                if (persistence)
                {
                    properties = channel.CreateBasicProperties();
                    properties.Persistent = persistence;
                }
                channel.BasicPublish(
                    normalExchangeParam.Exchange,
                    DefaultRoutingKey,
                    properties,
                    body
                );
            });

    private static Policy GetRetryPolicy(int retry) =>
        Policy
            .Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(
                retry,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, _) => throw ex
            );
}
