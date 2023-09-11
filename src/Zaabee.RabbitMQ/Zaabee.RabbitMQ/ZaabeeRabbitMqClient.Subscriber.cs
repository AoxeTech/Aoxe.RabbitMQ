namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    private const ushort DefaultPrefetchCount = 10;

    private readonly ConcurrentDictionary<string, IModel> _subscriberChannelDic = new();

    private void Subscribe<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        Func<Action<T?>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);
        
        if (dlx)
            ConsumeEvent(channel, resolve, queueParam.Queue);
        else
            ConsumeMessage(channel, resolve, queueParam.Queue);
    }

    private void Subscribe(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        Func<Action<byte[]>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);
        
        if (dlx)
            ConsumeEvent(channel, resolve, queueParam.Queue);
        else
            ConsumeMessage(channel, resolve, queueParam.Queue);
    }

    private void Subscribe<T>(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        Func<Func<T?, Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);
        
        if (dlx)
            ConsumeEvent(channel, resolve, queueParam.Queue);
        else
            ConsumeMessage(channel, resolve, queueParam.Queue);
    }

    private void Subscribe(
        ExchangeParam exchangeParam,
        QueueParam queueParam,
        Func<Func<byte[], Task>> resolve,
        ushort prefetchCount = DefaultPrefetchCount,
        int retry = 0,
        bool dlx = false)
    {
        var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);
        
        if (dlx)
            ConsumeEvent(channel, resolve, queueParam.Queue);
        else
            ConsumeMessage(channel, resolve, queueParam.Queue);
    }

    private void ConsumeEvent<T>(
        IModel channel,
        Func<Action<T?>> resolve,
        string queue)
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

    private void ConsumeEvent<T>(
        IModel channel,
        Func<Func<T?, Task>> resolve,
        string queue)
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
    
    private void ConsumeMessage<T>(
        IModel channel,
        Func<Action<T?>> resolve,
        string queue)
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

    private void ConsumeMessage<T>(
        IModel channel,
        Func<Func<T?, Task>> resolve,
        string queue)
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

    private IModel GetReceiverChannel(
        ExchangeParam? exchangeParam,
        QueueParam queueParam,
        ushort prefetchCount)
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
}