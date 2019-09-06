using System;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        private readonly ConcurrentDictionary<string, IModel> _subscriberChannelDic =
            new ConcurrentDictionary<string, IModel>();

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
            var methodFullName = GetQueueName(resolve);
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
            var methodFullName = GetQueueName(resolve);

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

        private IModel GetReceiverChannel(ExchangeParam exchangeParam, QueueParam queueParam,
            ushort prefetchCount)
        {
            return _subscriberChannelDic.GetOrAdd(queueParam.Queue, key =>
            {
                var channel = _conn.CreateModel();

                channel.QueueDeclare(queue: queueParam.Queue, durable: queueParam.Durable,
                    exclusive: queueParam.Exclusive, autoDelete: queueParam.AutoDelete,
                    arguments: queueParam.Arguments);

                if (exchangeParam != null)
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

        private void ConsumeEvent<T>(IModel channel, Action<T> handle, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var msg = _serializer.Deserialize<T>(ea.Body);
                    handle(msg);
                }
                catch (Exception ex)
                {
                    PublishDlx(ea, queue, ex);
                }
                finally
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private void ConsumeEvent<T>(IModel channel, Func<Action<T>> resolve, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var msg = _serializer.Deserialize<T>(ea.Body);
                    resolve.Invoke()(msg);
                }
                catch (Exception ex)
                {
                    PublishDlx(ea, queue, ex);
                }
                finally
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private void ConsumeMessage<T>(IModel channel, Action<T> handle, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
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
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private void ConsumeMessage<T>(IModel channel, Func<Action<T>> resolve, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
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
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private void PublishDlx(BasicDeliverEventArgs ea, string queue, Exception ex)
        {
            var inmostEx = ex.GetInmostException();

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
                    ExMsg = inmostEx.Message,
                    ExStack = inmostEx.StackTrace,
                    ThrowTime = DateTimeOffset.Now,
                    BodyString = _serializer.BytesToText(ea.Body)
                };

                deadLetterMsgChannel.BasicPublish(dlxExchangeParam.Exchange, routingKey, properties,
                    _serializer.Serialize(dlx));
            }
        }
    }
}