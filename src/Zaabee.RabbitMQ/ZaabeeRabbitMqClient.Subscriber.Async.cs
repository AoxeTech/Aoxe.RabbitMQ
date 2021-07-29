using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        #region Event

        public async Task ReceiveEventAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var eventName = GetTypeName(typeof(T));
            await SubscribeEventAsync(eventName, eventName, resolve, prefetchCount);
        }

        public async Task SubscribeEventAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var methodFullName = GetQueueName(resolve);
            await SubscribeEventAsync(methodFullName, resolve, prefetchCount);
        }

        public async Task SubscribeEventAsync<T>(string queue, Func<Action<T>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchange = GetTypeName(typeof(T));
            await SubscribeEventAsync(exchange, queue, resolve, prefetchCount);
        }

        public async Task SubscribeEventAsync<T>(string exchange, string queue, Func<Action<T>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchange};
            var queueParam = new QueueParam {Queue = queue};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            await ConsumeEventAsync(channel, resolve, queueParam.Queue);
        }

        #endregion

        #region Message

        public async Task ReceiveMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var messageName = GetTypeName(typeof(T));
            await SubscribeMessageAsync(messageName, messageName, resolve, prefetchCount);
        }

        public async Task SubscribeMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var methodFullName = GetQueueName(resolve);
            await SubscribeMessageAsync(methodFullName, resolve, prefetchCount);
        }

        public async Task SubscribeMessageAsync<T>(string queue, Func<Action<T>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchange = GetTypeName(typeof(T));
            await SubscribeMessageAsync(exchange, queue, resolve, prefetchCount);
        }

        public async Task SubscribeMessageAsync<T>(string exchange, string queue, Func<Action<T>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchange, Durable = false};
            var queueParam = new QueueParam {Queue = queue, Durable = false};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            await ConsumeEventAsync(channel, resolve, queueParam.Queue);
        }

        public async Task ListenMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchangeName = GetTypeName(typeof(T));
            var queueName = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";

            var exchangeParam = new ExchangeParam {Exchange = exchangeName, Durable = false};
            var queueParam = new QueueParam {Queue = queueName, Durable = false, Exclusive = true, AutoDelete = true};
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            await ConsumeMessageAsync(channel, resolve, queueParam.Queue);
        }

        #endregion

        #region Command

        public async Task ReceiveCommandAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var commandName = GetTypeName(typeof(T));
            await ReceiveCommandAsync(commandName, resolve, prefetchCount);
        }

        public async Task ReceiveCommandAsync<T>(string queue, Func<Action<T>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var queueParam = new QueueParam {Queue = queue};
            var channel = GetReceiverChannel(null, queueParam, prefetchCount);

            await ConsumeEventAsync(channel, resolve, queueParam.Queue);
        }

        #endregion

        private Task ConsumeEventAsync<T>(IModel channel, Func<Action<T>> resolve, string queue)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var msg = _serializer.DeserializeFromBytes<T>(ea.Body.ToArray());
                    resolve.Invoke()(msg);
                }
                catch (Exception ex)
                {
                    PublishDlx<T>(ea, queue, ex);
                }
                finally
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    await Task.Yield();
                }
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        private Task ConsumeMessageAsync<T>(IModel channel, Func<Action<T>> resolve, string queue)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var msg = _serializer.DeserializeFromBytes<T>(body.ToArray());
                    resolve.Invoke()(msg);
                }
                finally
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    await Task.Yield();
                }
            };
            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}