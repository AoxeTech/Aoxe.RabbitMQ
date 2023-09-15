namespace Zaabee.RabbitMQ;

public sealed partial class ZaabeeRabbitMqClient : IZaabeeRabbitMqClient
{
    private readonly IConnection _publishConn;
    private readonly IConnection _subscribeConn;
    private readonly IConnection _subscribeAsyncConn;
    private readonly IJsonSerializer _serializer;

    private readonly ConcurrentDictionary<Type, string> _topicNameDic = new();
    private readonly ConcurrentDictionary<string, IModel> _subscriberChannelDic = new();
    private readonly ConcurrentDictionary<string, IModel> _subscriberAsyncChannelDic = new();
    private const string DefaultRoutingKey = "#";

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

    private IModel GetConsumerChannel(
        ExchangeParam normalExchangeParam,
        QueueParam normalQueueParam,
        ExchangeParam? retryExchangeParam = null,
        ExchangeParam? dlxExchangeParam = null,
        QueueParam? dlxQueueParam = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount) =>
        _subscriberChannelDic.GetOrAdd(normalQueueParam.Queue, _ =>
        {
            var channel = GenerateChannel(_subscribeConn,
                normalExchangeParam,
                normalQueueParam,
                retryExchangeParam,
                dlxExchangeParam,
                dlxQueueParam);
            channel.BasicQos(0, prefetchCount, false);
            return channel;
        });

    private IModel GetConsumerAsyncChannel(
        ExchangeParam normalExchangeParam,
        QueueParam normalQueueParam,
        ExchangeParam? retryExchangeParam = null,
        ExchangeParam? dlxExchangeParam = null,
        QueueParam? dlxQueueParam = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount) =>
        _subscriberAsyncChannelDic.GetOrAdd(normalQueueParam.Queue, _ =>
        {
            var channel = GenerateChannel(_subscribeConn,
                normalExchangeParam,
                normalQueueParam,
                retryExchangeParam,
                dlxExchangeParam,
                dlxQueueParam);
            channel.BasicQos(0, prefetchCount, false);
            return channel;
        });

    private static IModel GenerateChannel(
        IConnection connection,
        ExchangeParam normalExchangeParam,
        QueueParam? normalQueueParam = null,
        ExchangeParam? retryExchangeParam = null,
        ExchangeParam? dlxExchangeParam = null,
        QueueParam? dlxQueueParam = null)
    {
        var channel = connection.CreateModel();

        Dictionary<string, object>? dlxArgs = null;

        if (dlxExchangeParam is not null && dlxQueueParam is not null)
            dlxArgs = DeclareDlxExchangeAndQueue(channel, dlxExchangeParam, dlxQueueParam);

        DeclareNormalExchangeAndQueue(channel, normalExchangeParam, normalQueueParam, dlxArgs);

        if (retryExchangeParam is not null && normalQueueParam is not null)
            DeclareRetryExchangeAndQueue(channel, retryExchangeParam, normalQueueParam.Queue);

        return channel;
    }

    /// <summary>
    /// Declare the normal exchange and queue, if the dlx queue is defined, then the normal queue will bind to the dlx queue.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="normalExchangeParam"></param>
    /// <param name="normalQueueParam"></param>
    /// <param name="dlxArgs"></param>
    private static void DeclareNormalExchangeAndQueue(
        IModel channel,
        ExchangeParam normalExchangeParam,
        QueueParam? normalQueueParam,
        Dictionary<string, object>? dlxArgs)
    {
        channel.ExchangeDeclare(
            exchange: normalExchangeParam.Exchange,
            type: normalExchangeParam.Type.ToString().ToLower(),
            durable: normalExchangeParam.Durable,
            autoDelete: normalExchangeParam.AutoDelete,
            arguments: normalExchangeParam.Arguments);

        if (normalQueueParam is null)
            return;

        if (dlxArgs is not null)
        {
            normalQueueParam.Arguments ??= new Dictionary<string, object>();
            foreach (var args in dlxArgs)
                normalQueueParam.Arguments.Add(args);
        }

        channel.QueueDeclare(
            queue: normalQueueParam.Queue,
            durable: normalQueueParam.Durable,
            exclusive: normalQueueParam.Exclusive,
            autoDelete: normalQueueParam.AutoDelete,
            arguments: normalQueueParam.Arguments);

        channel.QueueBind(
            queue: normalQueueParam.Queue,
            exchange: normalExchangeParam.Exchange,
            routingKey: DefaultRoutingKey);
    }

    private static void DeclareRetryExchangeAndQueue(
        IModel channel,
        ExchangeParam retryExchangeParam,
        string queueName)
    {
        channel.ExchangeDeclare(
            exchange: retryExchangeParam.Exchange,
            type: retryExchangeParam.Type.ToString().ToLower(),
            durable: retryExchangeParam.Durable,
            autoDelete: retryExchangeParam.AutoDelete,
            arguments: retryExchangeParam.Arguments);

        channel.QueueBind(
            queue: queueName,
            exchange: retryExchangeParam.Exchange,
            routingKey: DefaultRoutingKey);
    }

    /// <summary>
    /// Declare the dlx exchange and queue
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="dlxExchangeParam"></param>
    /// <param name="dlxQueueParam"></param>
    /// <returns></returns>
    private static Dictionary<string, object> DeclareDlxExchangeAndQueue(
        IModel channel,
        ExchangeParam dlxExchangeParam,
        QueueParam dlxQueueParam)
    {
        channel.ExchangeDeclare(
            exchange: dlxExchangeParam.Exchange,
            type: dlxExchangeParam.Type.ToString().ToLower(),
            durable: dlxExchangeParam.Durable,
            autoDelete: dlxExchangeParam.AutoDelete,
            arguments: dlxExchangeParam.Arguments);

        channel.QueueDeclare(
            queue: dlxQueueParam.Queue,
            durable: dlxQueueParam.Durable,
            exclusive: dlxQueueParam.Exclusive,
            autoDelete: dlxQueueParam.AutoDelete,
            arguments: dlxQueueParam.Arguments);

        channel.QueueBind(
            queue: dlxQueueParam.Queue,
            exchange: dlxExchangeParam.Exchange,
            routingKey: DefaultRoutingKey);

        return new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", dlxExchangeParam.Exchange }
        };
    }

    private static ExchangeParam GetExchangeParam(
        string topic,
        bool persistence,
        ExchangeRole exchangeRole = ExchangeRole.Normal) =>
        new()
        {
            Exchange = exchangeRole switch
            {
                ExchangeRole.Retry => $"{topic}[retry]",
                ExchangeRole.Dlx => $"{topic}[dlx]",
                _ => topic
            },
            Durable = persistence
        };

    private static QueueParam GetQueueParam(
        string queue,
        bool persistence,
        bool isExclusive = false,
        QueueRole queueRole = QueueRole.Normal)
    {
        var queueParam = new QueueParam
        {
            Queue = queueRole is QueueRole.Dlx ? $"{queue}[dlx]" : queue,
            Durable = persistence
        };
        queueParam.Arguments?.Add("x-queue-type", "quorum");
        if (!isExclusive) return queueParam;
        queueParam.Queue += $"[{Guid.NewGuid()}]";
        queueParam.Exclusive = true;
        queueParam.AutoDelete = true;
        return queueParam;
    }

    private string GetQueueName<T>(Func<Action<T>> resolve)
    {
        var handle = resolve();
        var messageName = GetTopicName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
    }

    private string GenerateQueueName<T>(Func<Func<T, Task>> resolve)
    {
        var handle = resolve();
        var messageName = GetTopicName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
    }

    private string GetTopicName(Type type) =>
        _topicNameDic.GetOrAdd(type,
            _ => type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault()
                is MessageVersionAttribute msgVerAttr
                ? $"{type}[{msgVerAttr.Version}]"
                : type.ToString());

    public void Dispose()
    {
        foreach (var keyValuePair in _subscriberChannelDic)
            keyValuePair.Value.Dispose();
        _publishConn.Dispose();
        _subscribeConn.Dispose();
        _subscribeAsyncConn.Dispose();
    }
}