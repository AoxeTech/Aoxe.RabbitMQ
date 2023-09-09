namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private void Publish<T>(
        T value,
        ExchangeParam exchangeParam,
        QueueParam? queueParam,
        bool persistence,
        int publishRetry = 3) =>
        Publish(_serializer.ToBytes(value), exchangeParam, queueParam, persistence, publishRetry);

    private void Publish(
        byte[] body,
        ExchangeParam exchangeParam,
        QueueParam? queueParam,
        bool persistence,
        int publishRetry = 3)
    {
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(publishRetry, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, _) => throw ex);

        policy.Execute(() =>
        {
            IBasicProperties? properties = null;
            using var channel = GetPublisherChannel(exchangeParam, queueParam);
            if (persistence)
            {
                properties = channel.CreateBasicProperties();
                properties.Persistent = persistence;
            }

            var routingKey = exchangeParam.Exchange;
            channel.BasicPublish(exchangeParam.Exchange, routingKey, properties, body);
        });
    }

    private IModel GetPublisherChannel(
        ExchangeParam exchangeParam,
        QueueParam? queueParam,
        string? routingKey = null)
    {
        var channel = _publishConn.CreateModel();

        channel.ExchangeDeclare(
            exchange: exchangeParam.Exchange,
            type: exchangeParam.Type.ToString().ToLower(),
            durable: exchangeParam.Durable,
            autoDelete: exchangeParam.AutoDelete,
            arguments: exchangeParam.Arguments);

        if (queueParam is null) return channel;

        channel.QueueDeclare(
            queue: queueParam.Queue,
            durable: queueParam.Durable,
            exclusive: queueParam.Exclusive,
            autoDelete: queueParam.AutoDelete,
            arguments: queueParam.Arguments);

        channel.QueueBind(
            queue: queueParam.Queue,
            exchange: exchangeParam.Exchange,
            routingKey: routingKey ?? queueParam.Queue);

        return channel;
    }
}