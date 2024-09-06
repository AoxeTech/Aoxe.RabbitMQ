using Aoxe.RabbitMQ.Client.Internal;

namespace Aoxe.RabbitMQ.Client;

public partial class AoxeRabbitMqClient
{
    private void Consume<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        ExchangeParam? dlxExchangeParam,
        QueueParam? dlxQueueParam,
        Func<Func<T?, ValueTask>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry
    )
    {
        var retryExchangeParam =
            consumeRetry > 0
                ? CreateExchangeParam(queueParam.Queue, true, ExchangeRole.Retry)
                : null;
        var channel = GetConsumerAsyncChannel(
            exchangeParam,
            queueParam,
            retryExchangeParam,
            dlxExchangeParam,
            dlxQueueParam,
            prefetchCount
        );
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (_, ea) =>
        {
            var consumerChannel = consumer.Model;
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                await resolve.Invoke()(msg);
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                if (retryExchangeParam is not null)
                {
                    var retryCount = GetRetryCount(ea.BasicProperties);
                    if (retryCount < consumeRetry)
                    {
                        IncreaseRetryCount(ea.BasicProperties);
                        consumerChannel.BasicPublish(
                            retryExchangeParam.Exchange,
                            DefaultRoutingKey,
                            ea.BasicProperties,
                            ea.Body
                        );
                        consumerChannel.BasicAck(ea.DeliveryTag, false);
                    }
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
        Func<Func<byte[], ValueTask>> resolve,
        ushort prefetchCount = Consts.DefaultPrefetchCount,
        int consumeRetry = Consts.DefaultConsumeRetry
    )
    {
        var retryExchangeParam =
            consumeRetry > 0
                ? CreateExchangeParam(queueParam.Queue, true, ExchangeRole.Retry)
                : null;
        var channel = GetConsumerAsyncChannel(
            exchangeParam,
            queueParam,
            retryExchangeParam,
            dlxExchangeParam,
            dlxQueueParam,
            prefetchCount
        );
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (_, ea) =>
        {
            var consumerChannel = consumer.Model;
            try
            {
                await resolve.Invoke()(ea.Body.ToArray());
                consumerChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                if (retryExchangeParam is not null)
                {
                    var retryCount = GetRetryCount(ea.BasicProperties);
                    if (retryCount < consumeRetry)
                    {
                        IncreaseRetryCount(ea.BasicProperties);
                        consumerChannel.BasicPublish(
                            retryExchangeParam.Exchange,
                            DefaultRoutingKey,
                            ea.BasicProperties,
                            ea.Body
                        );
                        consumerChannel.BasicAck(ea.DeliveryTag, false);
                    }
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
}
