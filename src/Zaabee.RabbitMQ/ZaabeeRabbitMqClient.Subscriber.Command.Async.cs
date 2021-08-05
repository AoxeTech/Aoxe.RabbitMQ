using System;
using System.Threading.Tasks;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        public async Task ReceiveCommandAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = DefaultPrefetchCount)
        {
            var queueParam = new QueueParam { Queue = GetTypeName(typeof(T)) };
            var channel = GetReceiverChannel(null, queueParam, prefetchCount);
            await ConsumeEventAsync(channel, resolve, queueParam.Queue);
        }

        public async Task ReceiveCommandAsync<T>(Func<Func<T, Task>> resolve,
            ushort prefetchCount = DefaultPrefetchCount)
        {
            var queueParam = new QueueParam { Queue = GetTypeName(typeof(T)) };
            var channel = GetReceiverChannel(null, queueParam, prefetchCount);
            await ConsumeEventAsync(channel, resolve, queueParam.Queue);
        }
    }
}