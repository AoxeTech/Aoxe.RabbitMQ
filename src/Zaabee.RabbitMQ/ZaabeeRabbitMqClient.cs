namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient : IZaabeeRabbitMqClient
{
    private readonly ushort _publishRetryCount;
    private readonly ushort _handleRetryCount;
    private readonly IConnection _publishConn;
    private readonly IConnection _subscribeConn;
    private readonly IConnection _subscribeAsyncConn;
    private readonly IJsonSerializer _serializer;

    private readonly ConcurrentDictionary<Type, string> _queueNameDic = new();

    public ZaabeeRabbitMqClient(ZaabeeRabbitMqOptions options)
    {
        _publishRetryCount = options.PublishRetryCount;
        _handleRetryCount = options.HandleRetryCount;
        if (options is null) throw new ArgumentNullException(nameof(options));
        if (options.Serializer is null) throw new ArgumentNullException(nameof(options.Serializer));
        if (options.Hosts.Count is 0) throw new ArgumentNullException(nameof(options.Hosts));

        var factory = new ConnectionFactory
        {
            RequestedHeartbeat = options.HeartBeat,
            AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled,
            NetworkRecoveryInterval = options.NetworkRecoveryInterval,
            RequestedConnectionTimeout = options.RequestedConnectionTimeout,
            SocketReadTimeout = options.SocketReadTimeout,
            SocketWriteTimeout = options.SocketWriteTimeout,
            UserName = options.UserName,
            Password = options.Password,
            VirtualHost = string.IsNullOrWhiteSpace(options.VirtualHost) ? "/" : options.VirtualHost,
        };

        var asyncFactory = new ConnectionFactory
        {
            RequestedHeartbeat = options.HeartBeat,
            AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled,
            NetworkRecoveryInterval = options.NetworkRecoveryInterval,
            RequestedConnectionTimeout = options.RequestedConnectionTimeout,
            SocketReadTimeout = options.SocketReadTimeout,
            SocketWriteTimeout = options.SocketWriteTimeout,
            UserName = options.UserName,
            Password = options.Password,
            VirtualHost = string.IsNullOrWhiteSpace(options.VirtualHost) ? "/" : options.VirtualHost,
            DispatchConsumersAsync = true
        };

        (_publishConn, _subscribeConn, _subscribeAsyncConn) =
            options.Hosts.Any()
                ? (factory.CreateConnection(options.Hosts),
                    factory.CreateConnection(options.Hosts),
                    asyncFactory.CreateConnection(options.Hosts))
                : (factory.CreateConnection(),
                    factory.CreateConnection(),
                    asyncFactory.CreateConnection());

        _serializer = options.Serializer;
    }

    // public void RepublishDeadLetterEvent<T>(string deadLetterQueueName, ushort prefetchCount = 1)
    // {
    //     var queueParam = new QueueParam { Queue = deadLetterQueueName };
    //     var channel = GetReceiverChannel(null, queueParam, prefetchCount);
    //
    //     var consumer = new EventingBasicConsumer(channel);
    //     consumer.Received += (_, ea) =>
    //     {
    //         try
    //         {
    //             var body = ea.Body;
    //             var msg = _serializer.FromBytes<DeadLetterMsg>(body.ToArray())!;
    //
    //             var republishExchangeParam =
    //                 new ExchangeParam { Exchange = $"republish-{deadLetterQueueName}", Durable = true };
    //             var republishQueueParam =
    //                 new QueueParam { Queue = FromDeadLetterName(deadLetterQueueName), Durable = true };
    //             using (var republishChannel = GetPublisherChannel(republishExchangeParam, republishQueueParam))
    //             {
    //                 var properties = republishChannel.CreateBasicProperties();
    //                 properties.Persistent = true;
    //                 var routingKey = republishExchangeParam.Exchange;
    //
    //                 var deadLetter = _serializer.FromText<T>(msg.BodyString);
    //
    //                 republishChannel.BasicPublish(republishExchangeParam.Exchange, routingKey, properties,
    //                     _serializer.ToBytes(deadLetter));
    //             }
    //
    //             channel.BasicAck(ea.DeliveryTag, false);
    //         }
    //         catch
    //         {
    //             channel.BasicNack(ea.DeliveryTag, false, true);
    //         }
    //     };
    //     channel.BasicConsume(queue: deadLetterQueueName, autoAck: false, consumer: consumer);
    // }

    private static ExchangeParam GetExchangeParam(
        string topic,
        bool persistence) =>
        new() { Exchange = topic, Durable = persistence };

    private static QueueParam GetQueueParam(
        string queue,
        bool persistence,
        SubscribeType subscribeType)
    {
        var queueParam = new QueueParam { Queue = queue, Durable = persistence };
        if (subscribeType is not SubscribeType.Listen) return queueParam;
        queueParam.Exclusive = true;
        queueParam.AutoDelete = true;
        return queueParam;
    }

    private string GetQueueName<T>(Func<Action<T>> resolve)
    {
        var handle = resolve();
        var messageName = GetTypeName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
    }

    private string GetQueueName<T>(Func<Func<T, Task>> resolve)
    {
        var handle = resolve();
        var messageName = GetTypeName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
    }

    private string GetTypeName(Type type) =>
        _queueNameDic.GetOrAdd(type,
            _ => type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault()
                is MessageVersionAttribute msgVerAttr
                ? $"{type}[{msgVerAttr.Version}]"
                : type.ToString());

    // private static string GetDeadLetterName(string name) =>
    //     $"dead-letter-{name}";
    //
    // private static string FromDeadLetterName(string deadLetterName) =>
    //     deadLetterName.Replace("dead-letter-", "");

    public void Dispose()
    {
        foreach (var keyValuePair in _subscriberChannelDic)
            keyValuePair.Value.Dispose();
        _publishConn.Dispose();
        _subscribeConn.Dispose();
        _subscribeAsyncConn.Dispose();
    }
}