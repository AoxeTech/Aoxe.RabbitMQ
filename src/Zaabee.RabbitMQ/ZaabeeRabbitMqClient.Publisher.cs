namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private void Publish<T>(
        ExchangeParam exchangeParam,
        QueueParam? queueParam,
        bool persistence,
        T value) =>
        Publish(exchangeParam, queueParam, persistence, _serializer.ToBytes(value));

    private void Publish(
        ExchangeParam exchangeParam,
        QueueParam? queueParam,
        bool persistence,
        byte[] body)
    {
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_publishRetryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, _) => throw ex);

        policy.Execute(() =>
        {
            using (var channel = GetPublisherChannel(exchangeParam, queueParam))
            {
                IBasicProperties? properties = null;
                if (persistence)
                {
                    properties = channel.CreateBasicProperties();
                    if (properties is not null)
                        properties.Persistent = persistence;
                }

                var routingKey = exchangeParam.Exchange;
                channel.BasicPublish(exchangeParam.Exchange, routingKey, properties, body);
            }
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