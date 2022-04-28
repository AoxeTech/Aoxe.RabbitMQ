namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private IModel GetPublisherChannel(ExchangeParam exchangeParam, QueueParam? queueParam, string? routingKey = null)
    {
        var channel = _publishConn.CreateModel();

        channel.ExchangeDeclare(exchange: exchangeParam.Exchange, type: exchangeParam.Type.ToString().ToLower(),
            durable: exchangeParam.Durable, autoDelete: exchangeParam.AutoDelete,
            arguments: exchangeParam.Arguments);

        if (queueParam is null) return channel;

        channel.QueueDeclare(queue: queueParam.Queue, durable: queueParam.Durable, exclusive: queueParam.Exclusive,
            autoDelete: queueParam.AutoDelete, arguments: queueParam.Arguments);
        channel.QueueBind(queue: queueParam.Queue, exchange: exchangeParam.Exchange,
            routingKey: routingKey ?? queueParam.Queue);

        return channel;
    }

    private void Publish<T>(ExchangeParam exchangeParam, QueueParam? queueParam, MessageType messageType, T value)
    {
        var body = _serializer.ToBytes(value);
        Publish(exchangeParam, queueParam, messageType, body);
    }

    private void Publish(ExchangeParam exchangeParam, QueueParam? queueParam, MessageType messageType, byte[] body)
    {
        using (var channel = GetPublisherChannel(exchangeParam, queueParam))
        {
            IBasicProperties? properties = null;
            if (messageType is MessageType.Event)
            {
                properties = channel.CreateBasicProperties();
                if (properties is not null)
                    properties.Persistent = true;
            }

            var routingKey = exchangeParam.Exchange;
            channel.BasicPublish(exchangeParam.Exchange, routingKey, properties, body);
        }
    }
}