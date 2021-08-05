using System;
using System.Threading.Tasks;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        public void ReceiveMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var messageName = GetTypeName(typeof(T));
            SubscribeMessage(messageName, messageName, resolve, prefetchCount);
        }

        public void ReceiveMessage<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var messageName = GetTypeName(typeof(T));
            SubscribeMessage(messageName, messageName, resolve, prefetchCount);
        }

        public void SubscribeMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var methodFullName = GetQueueName(resolve);
            SubscribeMessage(methodFullName, resolve, prefetchCount);
        }

        public void SubscribeMessage<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var methodFullName = GetQueueName(resolve);
            SubscribeMessage(methodFullName, resolve, prefetchCount);
        }

        public void SubscribeMessage<T>(string exchange, Func<Action<T>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var queue = GetQueueName(resolve);
            SubscribeMessage(exchange, queue, resolve, prefetchCount);
        }

        public void SubscribeMessage<T>(string exchange, Func<Func<T, Task>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var queue = GetQueueName(resolve);
            SubscribeMessage(exchange, queue, resolve, prefetchCount);
        }

        public void SubscribeMessage<T>(string exchange, string queue, Func<Action<T>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchangeParam = new ExchangeParam { Exchange = exchange, Durable = false };
            var queueParam = new QueueParam { Queue = queue, Durable = false };
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeEvent(channel, resolve, queueParam.Queue);
        }

        public void SubscribeMessage<T>(string exchange, string queue, Func<Func<T, Task>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchangeParam = new ExchangeParam { Exchange = exchange, Durable = false };
            var queueParam = new QueueParam { Queue = queue, Durable = false };
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeEvent(channel, resolve, queueParam.Queue);
        }

        public void ListenMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchangeName = GetTypeName(typeof(T));
            var queueName = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";

            var exchangeParam = new ExchangeParam { Exchange = exchangeName, Durable = false };
            var queueParam = new QueueParam { Queue = queueName, Durable = false, Exclusive = true, AutoDelete = true };
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeMessage(channel, resolve, queueParam.Queue);
        }

        public void ListenMessage<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var exchangeName = GetTypeName(typeof(T));
            var queueName = $"{GetQueueName(resolve)}[{Guid.NewGuid()}]";

            var exchangeParam = new ExchangeParam { Exchange = exchangeName, Durable = false };
            var queueParam = new QueueParam { Queue = queueName, Durable = false, Exclusive = true, AutoDelete = true };
            var channel = GetReceiverChannel(exchangeParam, queueParam, prefetchCount);

            ConsumeMessage(channel, resolve, queueParam.Queue);
        }
    }
}