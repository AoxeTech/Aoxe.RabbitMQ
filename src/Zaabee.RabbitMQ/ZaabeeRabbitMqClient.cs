namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient : IZaabeeRabbitMqClient
{
    private readonly IConnection _publishConn;
    private readonly IConnection _subscribeConn;
    private readonly IConnection _subscribeAsyncConn;
    private readonly ITextSerializer _serializer;

    private readonly ConcurrentDictionary<Type, string> _queueNameDic = new();

    public ZaabeeRabbitMqClient(ZaabeeRabbitMqOptions options)
    {
        if (options is null) throw new ArgumentNullException(nameof(options));
        if (options.Serializer is null) throw new ArgumentNullException(nameof(options.Serializer));
        if (options.Hosts.Count is 0) throw new ArgumentNullException(nameof(options.Hosts));

        var factory = new ConnectionFactory
        {
            RequestedHeartbeat = options.HeartBeat,
            AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled,
            NetworkRecoveryInterval = options.NetworkRecoveryInterval,
            UserName = options.UserName,
            Password = options.Password,
            VirtualHost = string.IsNullOrWhiteSpace(options.VirtualHost) ? "/" : options.VirtualHost,
        };

        var asyncFactory = new ConnectionFactory
        {
            RequestedHeartbeat = options.HeartBeat,
            AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled,
            NetworkRecoveryInterval = options.NetworkRecoveryInterval,
            UserName = options.UserName,
            Password = options.Password,
            VirtualHost = string.IsNullOrWhiteSpace(options.VirtualHost) ? "/" : options.VirtualHost,
            DispatchConsumersAsync = true
        };

        _publishConn = options.Hosts.Any()
            ? factory.CreateConnection(options.Hosts)
            : factory.CreateConnection();
        _subscribeConn = options.Hosts.Any()
            ? factory.CreateConnection(options.Hosts)
            : factory.CreateConnection();
        _subscribeAsyncConn = options.Hosts.Any()
            ? asyncFactory.CreateConnection(options.Hosts)
            : asyncFactory.CreateConnection();

        _serializer = options.Serializer;
    }

    public void RepublishDeadLetterEvent<T>(string deadLetterQueueName, ushort prefetchCount = 1)
    {
        var queueParam = new QueueParam { Queue = deadLetterQueueName };
        var channel = GetReceiverChannel(null, queueParam, prefetchCount);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, ea) =>
        {
            try
            {
                var body = ea.Body;
                var msg = _serializer.FromBytes<DeadLetterMsg>(body.ToArray())!;

                var republishExchangeParam =
                    new ExchangeParam { Exchange = $"republish-{deadLetterQueueName}", Durable = true };
                var republishQueueParam =
                    new QueueParam { Queue = FromDeadLetterName(deadLetterQueueName), Durable = true };
                using (var republishChannel = GetPublisherChannel(republishExchangeParam, republishQueueParam))
                {
                    var properties = republishChannel.CreateBasicProperties();
                    properties.Persistent = true;
                    var routingKey = republishExchangeParam.Exchange;

                    var deadLetter = _serializer.FromText<T>(msg.BodyString);

                    republishChannel.BasicPublish(republishExchangeParam.Exchange, routingKey, properties,
                        _serializer.ToBytes(deadLetter));
                }

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };
        channel.BasicConsume(queue: deadLetterQueueName, autoAck: false, consumer: consumer);
    }

    private string GetTypeName(Type type) =>
        _queueNameDic.GetOrAdd(type,
            _ => type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault()
                is MessageVersionAttribute msgVerAttr
                ? $"{type}[{msgVerAttr.Version}]"
                : type.ToString());

    private string GetQueueName<T>(Func<Action<T>> resolve)
    {
        var handle = resolve();
        return GetQueueName(handle);
    }

    private string GetQueueName<T>(Action<T> handle)
    {
        var messageName = GetTypeName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
    }

    private string GetQueueName<T>(Func<Func<T, Task>> resolve)
    {
        var handle = resolve();
        return GetQueueName(handle);
    }

    private string GetQueueName<T>(Func<T, Task> handle)
    {
        var messageName = GetTypeName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
    }

    private static string GetDeadLetterName(string name) =>
        $"dead-letter-{name}";

    private static string FromDeadLetterName(string deadLetterName) =>
        deadLetterName.Replace("dead-letter-", "");

    public void Dispose()
    {
        foreach (var keyValuePair in _subscriberChannelDic)
            keyValuePair.Value.Dispose();
        _publishConn.Dispose();
        _subscribeConn.Dispose();
        _subscribeAsyncConn.Dispose();
    }
}