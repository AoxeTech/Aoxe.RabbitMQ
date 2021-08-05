using System;
using System.Threading.Tasks;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        public void ReceiveCommand<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var queueParam = new QueueParam { Queue = GetTypeName(typeof(T)) };
            var channel = GetReceiverChannel(null, queueParam, prefetchCount);
            ConsumeEvent(channel, resolve, queueParam.Queue);
        }

        public void ReceiveCommand<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var queueParam = new QueueParam { Queue = GetTypeName(typeof(T)) };
            var channel = GetReceiverChannel(null, queueParam, prefetchCount);
            ConsumeEvent(channel, resolve, queueParam.Queue);
        }
    }
}