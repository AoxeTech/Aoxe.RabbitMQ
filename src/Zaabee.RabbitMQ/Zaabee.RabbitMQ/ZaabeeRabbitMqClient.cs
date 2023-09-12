namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient : IZaabeeRabbitMqClient
{
    private readonly IConnection _publishConn;
    private readonly IConnection _subscribeConn;
    private readonly IConnection _subscribeAsyncConn;
    private readonly IJsonSerializer _serializer;

    private readonly ConcurrentDictionary<Type, string> _queueNameDic = new();
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

    private IModel GetPublisherChannel(
        ExchangeParam normalExchangeParam,
        QueueParam normalQueueParam,
        ExchangeParam? dlxExchangeParam = null,
        QueueParam? dlxQueueParam = null)=>
        GenerateChannel(_publishConn,
            normalExchangeParam,
            normalQueueParam,
            dlxExchangeParam, 
            dlxQueueParam);

    private IModel GetConsumerChannel(
        ExchangeParam normalExchangeParam,
        QueueParam normalQueueParam,
        ExchangeParam? dlxExchangeParam = null,
        QueueParam? dlxQueueParam = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount) =>
        _subscriberChannelDic.GetOrAdd(normalQueueParam.Queue, _ =>
        {
            var channel = GenerateChannel(_subscribeConn,
                normalExchangeParam,
                normalQueueParam,
                dlxExchangeParam, 
                dlxQueueParam);
            channel.BasicQos(0, prefetchCount, false);
            return channel;
        });

    private IModel GetConsumerAsyncChannel(
        ExchangeParam normalExchangeParam,
        QueueParam normalQueueParam,
        ExchangeParam? dlxExchangeParam = null,
        QueueParam? dlxQueueParam = null,
        ushort prefetchCount = Consts.DefaultPrefetchCount)=>
        _subscriberAsyncChannelDic.GetOrAdd(normalQueueParam.Queue, _ =>
        {
            var channel = GenerateChannel(_subscribeConn,
                normalExchangeParam,
                normalQueueParam,
                dlxExchangeParam, 
                dlxQueueParam);
            channel.BasicQos(0, prefetchCount, false);
            return channel;
        });

    private static IModel GenerateChannel(
        IConnection connection,
        ExchangeParam normalExchangeParam,
        QueueParam? normalQueueParam = null,
        ExchangeParam? dlxExchangeParam = null,
        QueueParam? dlxQueueParam = null)
    {
        var channel = connection.CreateModel();

        Dictionary<string, object>? dlxArgs = null;

        if (dlxExchangeParam is not null && dlxQueueParam is not null)
            dlxArgs = DeclareDlxExchangeAndQueue(channel, dlxExchangeParam, dlxQueueParam);

        DeclareNormalExchangeAndQueue(channel, normalExchangeParam, normalQueueParam, dlxArgs);

        return channel;
    }

    /// <summary>
    /// Declare the normal exchange and queue, if the dlx queue is defined, then the normal queue will bind to the dlx queue.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="exchangeParam"></param>
    /// <param name="queueParam"></param>
    /// <param name="dlxArgs"></param>
    private static void DeclareNormalExchangeAndQueue(
        IModel channel,
        ExchangeParam exchangeParam,
        QueueParam? queueParam,
        Dictionary<string, object>? dlxArgs)
    {
        channel.ExchangeDeclare(
            exchange: exchangeParam.Exchange,
            type: exchangeParam.Type.ToString().ToLower(),
            durable: exchangeParam.Durable,
            autoDelete: exchangeParam.AutoDelete,
            arguments: exchangeParam.Arguments);

        if (queueParam is null)
            return;

        if (dlxArgs is not null)
        {
            queueParam.Arguments ??= new Dictionary<string, object>();
            foreach (var args in dlxArgs)
                queueParam.Arguments.Add(args);
        }

        channel.QueueDeclare(
            queue: queueParam.Queue,
            durable: queueParam.Durable,
            exclusive: queueParam.Exclusive,
            autoDelete: queueParam.AutoDelete,
            arguments: queueParam.Arguments);

        channel.QueueBind(
            queue: queueParam.Queue,
            exchange: exchangeParam.Exchange,
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
        bool persistence) =>
        new() { Exchange = topic, Durable = persistence };

    private static QueueParam GetQueueParam(
        string queue,
        bool persistence,
        SubscribeType subscribeType)
    {
        var queueParam = new QueueParam { Queue = queue, Durable = persistence };
        if (subscribeType is not SubscribeType.Listen) return queueParam;
        queueParam.Exclusive = false;
        queueParam.AutoDelete = true;
        queueParam.Arguments?.Add("x-queue-type", "quorum");
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

    public void Dispose()
    {
        foreach (var keyValuePair in _subscriberChannelDic)
            keyValuePair.Value.Dispose();
        _publishConn.Dispose();
        _subscribeConn.Dispose();
        _subscribeAsyncConn.Dispose();
    }
}