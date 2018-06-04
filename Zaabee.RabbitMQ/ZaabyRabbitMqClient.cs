using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Zaabee.RabbitMQ.Core;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ
{
    public class ZaabyRabbitMqClient : IMessageBus
    {
        private static IConnection _conn;
        private static ISerializer _serializer;
        private const ushort PrefetchCount = 10;

        private static readonly ConcurrentDictionary<string, IModel> SubscriberChannelDic =
            new ConcurrentDictionary<string, IModel>();

        private static readonly ConcurrentDictionary<Type, string> QueueNameDic =
            new ConcurrentDictionary<Type, string>();

        private static readonly object LockObj = new object();

        public ZaabyRabbitMqClient(MqConfig config, ISerializer serializer)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (config.Hosts.Count == 0) throw new ArgumentNullException(nameof(config.Hosts));
            if (_conn != null) return;
            lock (LockObj)
            {
                if (_conn != null) return;

                var factory = new ConnectionFactory
                {
                    RequestedHeartbeat = config.HeartBeat,
                    AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                    NetworkRecoveryInterval = config.NetworkRecoveryInterval,
                    UserName = config.UserName,
                    Password = config.Password,
                    VirtualHost = string.IsNullOrWhiteSpace(config.VirtualHost) ? "/" : config.VirtualHost,
                };

                _conn = config.Hosts.Any() ? factory.CreateConnection(config.Hosts) : factory.CreateConnection();

                _serializer = serializer;
            }
        }

        public void PublishEvent<T>(T @event)
        {
            var eventName = GetTypeName(typeof(T));
            var body = _serializer.Serialize(@event);
            PublishEvent(eventName, body);
        }

        public void PublishEvent(string eventName, byte[] body)
        {
            var exchangeParam = new ExchangeParam {Exchange = eventName};
            var queueParam = new QueueParam {Queue = eventName};
            using (var channel = CreatePublisherChannel(exchangeParam, queueParam))
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                var routingKey = queueParam.Queue;

                channel.BasicPublish(exchangeParam.Exchange, routingKey, properties, body);
            }
        }

        public void PublishMessage<T>(T message)
        {
            var messageName = GetTypeName(typeof(T));

            var exchangeParam = new ExchangeParam {Exchange = messageName, Durable = false};
            var queueParam = new QueueParam {Queue = messageName, Durable = false};
            using (var channel = CreatePublisherChannel(exchangeParam, queueParam))
            {
                var routingKey = queueParam.Queue;

                channel.BasicPublish(exchange: exchangeParam.Exchange, routingKey: routingKey, basicProperties: null,
                    body: _serializer.Serialize(message));
            }
        }

        public void PublishMessage(string messageName, byte[] body)
        {
            var exchangeParam = new ExchangeParam {Exchange = messageName, Durable = false};
            var queueParam = new QueueParam {Queue = messageName, Durable = false};
            using (var channel = CreatePublisherChannel(exchangeParam, queueParam))
            {
                var routingKey = queueParam.Queue;

                channel.BasicPublish(exchangeParam.Exchange, routingKey, null, body);
            }
        }

        public void ReceiveEvent<T>(Action<T> handle)
        {
            var eventName = GetTypeName(typeof(T));
            var queueParam = new QueueParam {Queue = eventName};
            var channel = GetReceiverChannel(null, queueParam, PrefetchCount);

            ConsumeEvent(channel, handle, eventName);
        }

        public void SubscribeEvent<T>(Action<T> handle)
        {
            var eventName = GetTypeName(typeof(T));
            var exchange = eventName;
            var methdoFullName = $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}.{eventName}";

            var exchangeParam = new ExchangeParam {Exchange = exchange};
            var queueParam = new QueueParam {Queue = methdoFullName};
            var channel = GetReceiverChannel(exchangeParam, queueParam, PrefetchCount);

            ConsumeEvent(channel, handle, queueParam.Queue);
        }

        public void RepublishDeadLetterEvent<T>()
        {
            var eventName = GetTypeName(typeof(T));
            var deadLetterName = GetDeadLetterName(eventName);
            var queueParam = new QueueParam {Queue = eventName};
            var channel = GetReceiverChannel(null, queueParam, 1);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var msg = _serializer.Deserialize<DeadLetterMsg>(body);

                var republishExchangeParam = new ExchangeParam {Exchange = $"republish-{eventName}"};
                using (var republishChannel = CreatePublisherChannel(republishExchangeParam, queueParam))
                {
                    var properties = republishChannel.CreateBasicProperties();
                    properties.Persistent = true;
                    var routingKey = republishExchangeParam.Exchange;

                    var deadLetter = _serializer.FromString<T>(msg.BodyString);

                    republishChannel.BasicPublish(republishExchangeParam.Exchange, routingKey, properties,
                        _serializer.Serialize(deadLetter));
                }

                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queue: deadLetterName, autoAck: false, consumer: consumer);
        }

        public void ReceiveMessage<T>(Action<T> handle)
        {
            var messageName = GetTypeName(typeof(T));
            var queueParam = new QueueParam {Queue = messageName, Durable = false};
            var channel = GetReceiverChannel(null, queueParam, PrefetchCount);

            ConsumeMessage(channel, handle, messageName);
        }

        public void SubscribeMessage<T>(Action<T> handle)
        {
            var messageName = GetTypeName(typeof(T));
            var exchange = messageName;
            var methdoFullName = $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}.{messageName}";

            var exchangeParam = new ExchangeParam {Exchange = exchange, Durable = false};
            var queueParam = new QueueParam {Queue = methdoFullName, Durable = false};
            var channel = GetReceiverChannel(exchangeParam, queueParam, PrefetchCount);

            ConsumeMessage(channel, handle, queueParam.Queue);
        }

        public void ListenMessage<T>(Action<T> handle)
        {
            var messageName = GetTypeName(typeof(T));
            var exchange = messageName;
            var methdoFullName =
                $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}.{messageName}.{Guid.NewGuid()}";

            var exchangeParam = new ExchangeParam {Exchange = exchange, Durable = false};
            var queueParam =
                new QueueParam {Queue = methdoFullName, Durable = false, Exclusive = true, AutoDelete = true};
            var channel = GetReceiverChannel(exchangeParam, queueParam, PrefetchCount);

            ConsumeMessage(channel, handle, queueParam.Queue);
        }

        private static IModel CreatePublisherChannel(ExchangeParam exchangeParam, QueueParam queueParam)
        {
            var channel = _conn.CreateModel();

            exchangeParam.Exchange = exchangeParam.Exchange ?? "UndefinedExchangeName";
            queueParam.Queue = queueParam.Queue ?? "UndefinedQueueName";

            channel.ExchangeDeclare(exchange: exchangeParam.Exchange, type: exchangeParam.Type.ToString().ToLower(),
                durable: exchangeParam.Durable, autoDelete: exchangeParam.AutoDelete,
                arguments: exchangeParam.Arguments);

            channel.QueueDeclare(queue: queueParam.Queue, durable: queueParam.Durable,
                exclusive: queueParam.Exclusive, autoDelete: queueParam.AutoDelete,
                arguments: queueParam.Arguments);

            channel.QueueBind(queue: queueParam.Queue, exchange: exchangeParam.Exchange,
                routingKey: queueParam.Queue);

            return channel;
        }

        private static IModel GetReceiverChannel(ExchangeParam exchangeParam, QueueParam queueParam,
            ushort prefetchCount)
        {
            return SubscriberChannelDic.GetOrAdd(queueParam.Queue, key =>
            {
                var channel = _conn.CreateModel();

                queueParam.Queue = queueParam.Queue ?? "UndefinedQueueName";

                channel.QueueDeclare(queue: queueParam.Queue, durable: queueParam.Durable,
                    exclusive: queueParam.Exclusive, autoDelete: queueParam.AutoDelete,
                    arguments: queueParam.Arguments);

                if (exchangeParam != null)
                {
                    exchangeParam.Exchange = exchangeParam.Exchange ?? "UndefinedExchangeName";
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

        private void ConsumeEvent<T>(IModel channel, Action<T> handle, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Task.Run(() =>
                {
                    var body = ea.Body;
                    var msg = _serializer.Deserialize<T>(body);

                    try
                    {
                        handle(msg);
                    }
                    catch (Exception ex)
                    {
                        var innestEx = ex.GetInnestException();
                        PublishEvent(GetDeadLetterName(queue), _serializer.Serialize(new DeadLetterMsg
                        {
                            QueueName = queue,
                            BodyString = _serializer.ToString(msg),
                            ExMsg = innestEx.Message,
                            ExStack = innestEx.StackTrace,
                            ThrowTime = DateTimeOffset.Now
                        }));
                    }
                    finally
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                });
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private void ConsumeMessage<T>(IModel channel, Action<T> handle, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        var body = ea.Body;
                        var msg = _serializer.Deserialize<T>(body);
                        handle(msg);
                    }
                    finally
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                });
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private static string GetTypeName(Type type)
        {
            return QueueNameDic.GetOrAdd(type,
                key => !(type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault() is
                    MessageVersionAttribute msgVerAttr)
                    ? type.ToString()
                    : $"{type.ToString()}[{msgVerAttr.Version}]");
        }

        private static string GetDeadLetterName(string name)
        {
            return $"dead-letter-{name}";
        }
    }
}