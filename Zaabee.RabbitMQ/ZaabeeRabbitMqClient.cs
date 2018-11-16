using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ
{
    public class ZaabeeRabbitMqClient : IZaabeeRabbitMqClient
    {
        private readonly IConnection _conn;
        private readonly ISerializer _serializer;

        private readonly ConcurrentDictionary<string, IModel> _subscriberChannelDic =
            new ConcurrentDictionary<string, IModel>();

        private readonly ConcurrentDictionary<Type, string> _queueNameDic =
            new ConcurrentDictionary<Type, string>();

        public ZaabeeRabbitMqClient(MqConfig config, ISerializer serializer)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (config.Hosts.Count == 0) throw new ArgumentNullException(nameof(config.Hosts));

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

        public void PublishEvent<T>(T @event)
        {
            var exchangeName = GetTypeName(typeof(T));
            PublishEvent(exchangeName, @event);
        }

        public void PublishEvent<T>(string exchangeName, T @event)
        {
            var body = _serializer.Serialize(@event);
            PublishEvent(exchangeName, body);
        }

        public void PublishEvent(string exchangeName, byte[] body)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchangeName};
            using (var channel = GetPublisherChannel(exchangeParam, null))
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                var routingKey = exchangeParam.Exchange;

                channel.BasicPublish(exchangeParam.Exchange, routingKey, properties, body);
            }
        }

        public void PublishMessage<T>(T message)
        {
            var exchangeName = GetTypeName(typeof(T));
            PublishMessage(exchangeName, message);
        }

        public void PublishMessage<T>(string exchangeName, T message)
        {
            var body = _serializer.Serialize(message);
            PublishMessage(exchangeName, body);
        }

        public void PublishMessage(string exchangeName, byte[] body)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchangeName, Durable = false};
            using (var channel = GetPublisherChannel(exchangeParam, null))
            {
                var routingKey = exchangeParam.Exchange;
                channel.BasicPublish(exchangeParam.Exchange, routingKey, null, body);
            }
        }

        public async Task PublishEventAsync<T>(T @event)
        {
            await Task.Run(() => { PublishEvent(@event); });
        }

        public async Task PublishEventAsync<T>(string exchangeName, T @event)
        {
            await Task.Run(() => { PublishEvent(exchangeName, @event); });
        }

        public async Task PublishEventAsync(string exchangeName, byte[] body)
        {
            await Task.Run(() => { PublishEvent(exchangeName, body); });
        }

        public async Task PublishMessageAsync<T>(T message)
        {
            await Task.Run(() => { PublishMessage(message); });
        }

        public async Task PublishMessageAsync<T>(string exchangeName, T message)
        {
            await Task.Run(() => { PublishMessage(exchangeName, message); });
        }

        public async Task PublishMessageAsync(string exchangeName, byte[] body)
        {
            await Task.Run(() => { PublishMessage(exchangeName, body); });
        }

        public void ReceiveEvent<T>(Action<T> handle, ushort prefetchCount = 10)
        {
            var eventName = GetTypeName(typeof(T));
            var exchangeParam = new ExchangeParam {Exchange = eventName};
            var queueParam = new QueueParam {Queue = eventName};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeEvent(channel, handle, eventName);
        }

        public void ReceiveEvent<T>(Func<Action<T>> resolve, ushort prefetchCount = 10)
        {
            var eventName = GetTypeName(typeof(T));
            var exchangeParam = new ExchangeParam {Exchange = eventName};
            var queueParam = new QueueParam {Queue = eventName};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeEvent(channel, resolve, eventName);
        }

        public void SubscribeEvent<T>(Action<T> handle, ushort prefetchCount = 10)
        {
            var methodFullName = GetQueueName(handle);
            SubscribeEvent(methodFullName, handle, prefetchCount);
        }

        public void SubscribeEvent<T>(Func<Action<T>> resolve, ushort prefetchCount = 10)
        {
            var handle = resolve();
            var methodFullName = GetQueueName(handle);
            SubscribeEvent(methodFullName, resolve, prefetchCount);
        }

        public void SubscribeEvent<T>(string queue, Action<T> handle, ushort prefetchCount = 10)
        {
            var exchange = GetTypeName(typeof(T));
            SubscribeEvent(exchange, queue, handle, prefetchCount);
        }

        public void SubscribeEvent<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10)
        {
            var exchange = GetTypeName(typeof(T));
            SubscribeEvent(exchange, queue, resolve, prefetchCount);
        }

        public void SubscribeEvent<T>(string exchange, string queue, Action<T> handle, ushort prefetchCount = 10)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchange};
            var queueParam = new QueueParam {Queue = queue};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeEvent(channel, handle, queueParam.Queue);
        }

        public void SubscribeEvent<T>(string exchange, string queue, Func<Action<T>> resolve, ushort prefetchCount = 10)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchange};
            var queueParam = new QueueParam {Queue = queue};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeEvent(channel, resolve, queueParam.Queue);
        }

        public void RepublishDeadLetterEvent<T>(string deadLetterQueueName, ushort prefetchCount = 1)
        {
            var queueParam = new QueueParam {Queue = deadLetterQueueName};
            var channel = GetReceiverChannel(null, queueParam, prefetchCount);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var msg = _serializer.Deserialize<DeadLetterMsg>(body);

                var republishExchangeParam =
                    new ExchangeParam {Exchange = $"republish-{deadLetterQueueName}", Durable = true};
                var republishQueueParam =
                    new QueueParam {Queue = FromDeadLetterName(deadLetterQueueName), Durable = true};
                using (var republishChannel = GetPublisherChannel(republishExchangeParam, republishQueueParam))
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
            channel.BasicConsume(queue: deadLetterQueueName, autoAck: false, consumer: consumer);
        }

        public void ReceiveMessage<T>(Action<T> handle, ushort prefetchCount = 10)
        {
            var messageName = GetTypeName(typeof(T));
            var exchangeParam = new ExchangeParam {Exchange = messageName, Durable = false};
            var queueParam = new QueueParam {Queue = messageName, Durable = false};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeMessage(channel, handle, messageName);
        }

        public void ReceiveMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = 10)
        {
            var messageName = GetTypeName(typeof(T));
            var exchangeParam = new ExchangeParam {Exchange = messageName, Durable = false};
            var queueParam = new QueueParam {Queue = messageName, Durable = false};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeMessage(channel, resolve, messageName);
        }

        public void SubscribeMessage<T>(Action<T> handle, ushort prefetchCount = 10)
        {
            var methodFullName = GetQueueName(handle);

            SubscribeMessage(methodFullName, handle, prefetchCount);
        }

        public void SubscribeMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = 10)
        {
            var handle = resolve();
            var methodFullName = GetQueueName(handle);

            SubscribeMessage(methodFullName, resolve, prefetchCount);
        }

        public void SubscribeMessage<T>(string queue, Action<T> handle, ushort prefetchCount = 10)
        {
            var messageName = GetTypeName(typeof(T));
            SubscribeMessage(messageName, queue, handle, prefetchCount);
        }

        public void SubscribeMessage<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10)
        {
            var messageName = GetTypeName(typeof(T));
            SubscribeMessage(messageName, queue, resolve, prefetchCount);
        }

        public void SubscribeMessage<T>(string exchange, string queue, Action<T> handle, ushort prefetchCount = 10)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchange, Durable = false};
            var queueParam = new QueueParam {Queue = queue, Durable = false};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeEvent(channel, handle, queueParam.Queue);
        }

        public void SubscribeMessage<T>(string exchange, string queue, Func<Action<T>> resolve,
            ushort prefetchCount = 10)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchange, Durable = false};
            var queueParam = new QueueParam {Queue = queue, Durable = false};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeEvent(channel, resolve, queueParam.Queue);
        }

        public void ListenMessage<T>(Action<T> handle, ushort prefetchCount = 10)
        {
            var exchangeName = GetTypeName(typeof(T));
            var methodFullName =
                $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{exchangeName}][{Guid.NewGuid()}]";

            var exchangeParam = new ExchangeParam {Exchange = exchangeName, Durable = false};
            var queueParam =
                new QueueParam {Queue = methodFullName, Durable = false, Exclusive = true, AutoDelete = true};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeMessage(channel, handle, queueParam.Queue);
        }

        private IModel GetPublisherChannel(ExchangeParam exchangeParam, QueueParam queueParam)
        {
            var channel = _conn.CreateModel();

            exchangeParam.Exchange = exchangeParam.Exchange ?? "UndefinedExchangeName";

            channel.ExchangeDeclare(exchange: exchangeParam.Exchange, type: exchangeParam.Type.ToString().ToLower(),
                durable: exchangeParam.Durable, autoDelete: exchangeParam.AutoDelete,
                arguments: exchangeParam.Arguments);

            if (queueParam == null) return channel;

            queueParam.Queue = queueParam.Queue ?? "UndefinedQueueName";
            channel.QueueDeclare(queue: queueParam.Queue, durable: queueParam.Durable,
                exclusive: queueParam.Exclusive, autoDelete: queueParam.AutoDelete,
                arguments: queueParam.Arguments);
            channel.QueueBind(queue: queueParam.Queue, exchange: exchangeParam.Exchange,
                routingKey: queueParam.Queue);

            return channel;
        }

        private IModel GetReceiverChannel(ExchangeParam exchangeParam, QueueParam queueParam,
            ushort prefetchCount)
        {
            return _subscriberChannelDic.GetOrAdd(queueParam.Queue, key =>
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
                    try
                    {
                        var msg = _serializer.Deserialize<T>(ea.Body);
                        handle(msg);
                    }
                    catch (Exception ex)
                    {
                        var innermostEx = ex.GetInnestException();

                        var dlxName = GetDeadLetterName(queue);
                        var dlxExchangeParam = new ExchangeParam {Exchange = dlxName};
                        var dlxQueueParam = new QueueParam {Queue = dlxName};

                        using (var deadLetterMsgChannel = GetPublisherChannel(dlxExchangeParam, dlxQueueParam))
                        {
                            var properties = deadLetterMsgChannel.CreateBasicProperties();
                            properties.Persistent = true;
                            var routingKey = dlxExchangeParam.Exchange;

                            var dlx = new DeadLetterMsg
                            {
                                QueueName = queue,
                                ExMsg = innermostEx.Message,
                                ExStack = innermostEx.StackTrace,
                                ThrowTime = DateTimeOffset.Now,
                                BodyString = _serializer.BytesToString(ea.Body)
                            };

                            deadLetterMsgChannel.BasicPublish(dlxExchangeParam.Exchange, routingKey, properties,
                                _serializer.Serialize(dlx));
                        }
                    }
                    finally
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                });
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private void ConsumeEvent<T>(IModel channel, Func<Action<T>> resolve, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        var msg = _serializer.Deserialize<T>(ea.Body);
                        resolve.Invoke()(msg);
                    }
                    catch (Exception ex)
                    {
                        var innermostEx = ex.GetInnestException();

                        var dlxName = GetDeadLetterName(queue);
                        var dlxExchangeParam = new ExchangeParam {Exchange = dlxName};
                        var dlxQueueParam = new QueueParam {Queue = dlxName};

                        using (var deadLetterMsgChannel = GetPublisherChannel(dlxExchangeParam, dlxQueueParam))
                        {
                            var properties = deadLetterMsgChannel.CreateBasicProperties();
                            properties.Persistent = true;
                            var routingKey = dlxExchangeParam.Exchange;

                            var dlx = new DeadLetterMsg
                            {
                                QueueName = queue,
                                ExMsg = innermostEx.Message,
                                ExStack = innermostEx.StackTrace,
                                ThrowTime = DateTimeOffset.Now,
                                BodyString = _serializer.BytesToString(ea.Body)
                            };

                            deadLetterMsgChannel.BasicPublish(dlxExchangeParam.Exchange, routingKey, properties,
                                _serializer.Serialize(dlx));
                        }
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

        private void ConsumeMessage<T>(IModel channel, Func<Action<T>> resolve, string queue)
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
                        resolve.Invoke()(msg);
                    }
                    finally
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                });
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private string GetTypeName(Type type)
        {
            return _queueNameDic.GetOrAdd(type,
                key => !(type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault() is
                    MessageVersionAttribute msgVerAttr)
                    ? type.ToString()
                    : $"{type.ToString()}[{msgVerAttr.Version}]");
        }

        private string GetQueueName<T>(Action<T> handle)
        {
            var messageName = GetTypeName(typeof(T));
            return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
        }

        private static string GetDeadLetterName(string name)
        {
            return $"dead-letter-{name}";
        }

        private static string FromDeadLetterName(string deadLetterName)
        {
            return deadLetterName.Replace("dead-letter-", "");
        }

        public void Dispose()
        {
            foreach (var keyValuePair in _subscriberChannelDic)
                keyValuePair.Value.Dispose();

            _conn.Dispose();
        }
    }
}