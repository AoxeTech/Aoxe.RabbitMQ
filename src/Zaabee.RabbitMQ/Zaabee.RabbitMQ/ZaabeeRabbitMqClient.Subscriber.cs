namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private const string RetryCount = "retry-count";

    private void Consume<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Action<T?>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
    {
        var retryExchangeParam = consumeRetry > 0
            ? GetExchangeParam(queueParam.Queue, true, ExchangeRole.Retry)
            : null;
        var channel = GetConsumerChannel(
            exchangeParam,
            queueParam,
            retryExchangeParam,
            dlxExchangeParam,
            dlxQueueParam,
            prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
        return;

        void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
        {
            var eventingBasicConsumer = (EventingBasicConsumer)model;
            var consumerChannel = eventingBasicConsumer.Model;
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                resolve.Invoke()(msg);
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                IncreaseRetryCount(ea.BasicProperties);
                if (retryCount < consumeRetry && retryExchangeParam is not null)
                {
                    consumerChannel.BasicPublish(retryExchangeParam.Exchange, DefaultRoutingKey, ea.BasicProperties, ea.Body);
                    consumerChannel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    consumerChannel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
        }
    }

    private void Consume(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
    {
        var retryExchangeParam = consumeRetry > 0
            ? GetExchangeParam(queueParam.Queue, true, ExchangeRole.Retry)
            : null;
        var channel = GetConsumerChannel(
            exchangeParam,
            queueParam,
            retryExchangeParam,
            dlxExchangeParam,
            dlxQueueParam,
            prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
        return;

        void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
        {
            var eventingBasicConsumer = (EventingBasicConsumer)model;
            var consumerChannel = eventingBasicConsumer.Model;
            try
            {
                resolve.Invoke()(ea.Body.ToArray());
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                IncreaseRetryCount(ea.BasicProperties);
                if (retryCount < consumeRetry && retryExchangeParam is not null)
                {
                    consumerChannel.BasicPublish(retryExchangeParam.Exchange, DefaultRoutingKey, ea.BasicProperties, ea.Body);
                    consumerChannel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    consumerChannel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
        }
    }

    private void Consume<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
    {
        var retryExchangeParam = consumeRetry > 0
            ? GetExchangeParam(queueParam.Queue, true, ExchangeRole.Retry)
            : null;
        var channel = GetConsumerAsyncChannel(
            exchangeParam,
            queueParam,
            retryExchangeParam,
            dlxExchangeParam,
            dlxQueueParam,
            prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var eventingBasicConsumer = (EventingBasicConsumer)model;
            var consumerChannel = eventingBasicConsumer.Model;
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                await resolve.Invoke()(msg);
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                IncreaseRetryCount(ea.BasicProperties);
                if (retryCount < consumeRetry && retryExchangeParam is not null)
                {
                    consumerChannel.BasicPublish(retryExchangeParam.Exchange, DefaultRoutingKey, ea.BasicProperties, ea.Body);
                    consumerChannel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    consumerChannel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
            finally
            {
                await Task.Yield();
            }
        };
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
    }

    private void Consume(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry)
    {
        var retryExchangeParam = consumeRetry > 0
            ? GetExchangeParam(queueParam.Queue, true, ExchangeRole.Retry)
            : null;
        var channel = GetConsumerAsyncChannel(
            exchangeParam,
            queueParam,
            dlxExchangeParam,
            retryExchangeParam,
            dlxQueueParam,
            prefetchCount);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var eventingBasicConsumer = (EventingBasicConsumer)model;
            var consumerChannel = eventingBasicConsumer.Model;
            try
            {
                await resolve.Invoke()(ea.Body.ToArray());
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                var retryCount = GetRetryCount(ea.BasicProperties);
                IncreaseRetryCount(ea.BasicProperties);
                if (retryCount < consumeRetry && retryExchangeParam is not null)
                {
                    consumerChannel.BasicPublish(retryExchangeParam.Exchange, DefaultRoutingKey, ea.BasicProperties, ea.Body);
                    consumerChannel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    consumerChannel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
            finally
            {
                await Task.Yield();
            }
        };
        channel.BasicConsume(queue: queueParam.Queue, autoAck: false, consumer: consumer);
    }

    private static int GetRetryCount(IBasicProperties properties)
    {
        properties.Headers ??= new Dictionary<string, object>();
        if (properties.Headers.TryGetValue(RetryCount, out var retryCount))
            return (int)retryCount;
        retryCount = 0;
        properties.Headers.Add(RetryCount, retryCount);
        return (int)retryCount;
    }

    private void IncreaseRetryCount(IBasicProperties properties)
    {
        properties.Headers ??= new Dictionary<string, object>();
        if (!properties.Headers.TryGetValue(RetryCount, out var retryCount))
            properties.Headers.Add(RetryCount, 0);
        var count = (int)retryCount;
        properties.Headers[RetryCount] = ++count;
    }
}