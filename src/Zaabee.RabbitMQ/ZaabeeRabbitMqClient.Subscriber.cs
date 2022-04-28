namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private const ushort DefaultPrefetchCount = 10;

    private readonly ConcurrentDictionary<string, IModel> _subscriberChannelDic = new();

    private void Subscribe<T>(ExchangeParam exchangeParam,
        QueueParam queueParam,
        Func<Action<T?>> resolve,
        MessageType messageType,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);
        switch (messageType)
        {
            case MessageType.Message:
                ConsumeMessage(channel, resolve, queueParam.Queue);
                break;
            case MessageType.Event:
                ConsumeEvent(channel, resolve, queueParam.Queue);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
        }
    }

    private void Subscribe<T>(ExchangeParam exchangeParam,
        QueueParam queueParam,
        Func<Func<T?, Task>> resolve,
        MessageType messageType,
        ushort prefetchCount = DefaultPrefetchCount)
    {
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);
        switch (messageType)
        {
            case MessageType.Message:
                ConsumeMessage(channel, resolve, queueParam.Queue);
                break;
            case MessageType.Event:
                ConsumeEvent(channel, resolve, queueParam.Queue);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
        }
    }

    private void ConsumeEvent<T>(IModel channel, Func<Action<T?>> resolve, string queue)
    {
        var consumer = new EventingBasicConsumer(channel);

        void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var msg = _serializer.FromBytes<T>(ea.Body.ToArray());
                resolve.Invoke()(msg);
            }
            catch (Exception ex)
            {
                PublishDlx<T>(ea, queue, ex);
            }
            finally
            {
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }

        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
    }

    private void ConsumeEvent<T>(IModel channel, Func<Func<T?, Task>> resolve, string queue)
    {
        var consumer = new EventingBasicConsumer(channel);

        async void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var msg = _serializer.FromBytes<T>(ea.Body.ToArray());
                await resolve.Invoke()(msg);
            }
            catch (Exception ex)
            {
                PublishDlx<T>(ea, queue, ex);
            }
            finally
            {
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }

        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
    }

    private void ConsumeMessage<T>(IModel channel, Func<Action<T?>> resolve, string queue)
    {
        var consumer = new EventingBasicConsumer(channel);

        void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                resolve.Invoke()(msg);
            }
            finally
            {
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }

        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
    }

    private void ConsumeMessage<T>(IModel channel, Func<Func<T?, Task>> resolve, string queue)
    {
        var consumer = new EventingBasicConsumer(channel);

        async void OnConsumerOnReceived(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<T>(body.ToArray());
                await resolve.Invoke()(msg);
            }
            finally
            {
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }

        consumer.Received += OnConsumerOnReceived;
        channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
    }

    private IModel GetReceiverChannel(ExchangeParam? exchangeParam, QueueParam queueParam, ushort prefetchCount)
    {
        return _subscriberChannelDic.GetOrAdd(queueParam.Queue, _ =>
        {
            var channel = _subscribeConn.CreateModel();

            channel.QueueDeclare(queue: queueParam.Queue, durable: queueParam.Durable,
                exclusive: queueParam.Exclusive, autoDelete: queueParam.AutoDelete,
                arguments: queueParam.Arguments);

            if (exchangeParam is not null)
            {
                channel.ExchangeDeclare(exchange: exchangeParam.Exchange,
                    type: exchangeParam.Type.ToString().ToLower(),
                    durable: exchangeParam.Durable, autoDelete: exchangeParam.AutoDelete,
                    arguments: exchangeParam.Arguments);
                channel.QueueBind(queue: queueParam.Queue, exchange: exchangeParam.Exchange,
                    routingKey: queueParam.Queue);
            }

            channel.BasicQos(0, prefetchCount, false);
            return channel;
        });
    }

    private void PublishDlx<T>(BasicDeliverEventArgs ea, string queue, Exception ex)
    {
        var inmostEx = ex.GetInmostException();

        var dlxName = GetDeadLetterName(queue);
        var dlxExchangeParam = new ExchangeParam { Exchange = dlxName };
        var dlxQueueParam = new QueueParam { Queue = dlxName };

        using (var deadLetterMsgChannel = GetPublisherChannel(dlxExchangeParam, dlxQueueParam))
        {
            var properties = deadLetterMsgChannel.CreateBasicProperties();
            properties.Persistent = true;
            var routingKey = dlxExchangeParam.Exchange;

            var dlx = new DeadLetterMsg
            {
                QueueName = queue,
                ExMsg = inmostEx.Message,
                ExStack = inmostEx.StackTrace,
                ThrowTime = DateTimeOffset.Now,
                BodyString = _serializer.ToText(_serializer.FromBytes<T>(ea.Body.ToArray()))
            };

            deadLetterMsgChannel.BasicPublish(dlxExchangeParam.Exchange, routingKey, properties,
                _serializer.ToBytes(dlx));
        }
    }
}