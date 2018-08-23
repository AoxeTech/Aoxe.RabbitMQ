using System;

namespace Zaabee.RabbitMQ.Abstractions
{
        public interface IZaabeeRabbitMqClient
        {
                void PublishEvent<T>(T @event);
                void PublishEvent<T>(string exchangeName, T @event);
                void PublishEvent(string exchangeName, byte[] body);
                void PublishMessage<T>(T message);
                void PublishMessage<T>(string exchangeName, T message);
                void PublishMessage(string exchangeName, byte[] body);

                /// <summary>
                /// The subscriber cluster will receive the event by the default queue.
                /// </summary>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void ReceiveEvent<T>(Action<T> handle, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the event by the default queue.
                /// </summary>
                /// <param name="resolve"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void ReceiveEvent<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the event by its own queue.
                /// </summary>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeEvent<T>(Action<T> handle, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the event by its own queue.
                /// </summary>
                /// <param name="resolve"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeEvent<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the event by the specified queue.
                /// </summary>
                /// <param name="queue"></param>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeEvent<T>(string queue, Action<T> handle, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the event by the specified queue.
                /// </summary>
                /// <param name="queue"></param>
                /// <param name="resolve"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeEvent<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the event by the specified exchange and queue.
                /// </summary>
                /// <param name="exchange"></param>
                /// <param name="queue"></param>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeEvent<T>(string exchange, string queue, Action<T> handle, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the event by the specified exchange and queue.
                /// </summary>
                /// <param name="exchange"></param>
                /// <param name="queue"></param>
                /// <param name="resolve"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeEvent<T>(string exchange, string queue, Func<Action<T>> resolve,
                        ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the message by the default queue.
                /// </summary>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void ReceiveMessage<T>(Action<T> handle, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the message by the default queue.
                /// </summary>
                /// <param name="resolve"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void ReceiveMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the message by its own queue.
                /// </summary>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeMessage<T>(Action<T> handle, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the message by its own queue.
                /// </summary>
                /// <param name="resolve"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeMessage<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the message by the specified queue.
                /// </summary>
                /// <param name="queue"></param>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeMessage<T>(string queue, Action<T> handle, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the message by the specified queue.
                /// </summary>
                /// <param name="queue"></param>
                /// <param name="resolve"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeMessage<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber cluster will receive the message by the specified exchange and queue.
                /// </summary>
                /// <param name="exchange"></param>
                /// <param name="queue"></param>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeMessage<T>(string exchange, string queue, Action<T> handle, ushort prefetchCount = 10);

                /// <summary>
                /// 
                /// </summary>
                /// <param name="exchange"></param>
                /// <param name="queue"></param>
                /// <param name="resolve"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void SubscribeMessage<T>(string exchange, string queue, Func<Action<T>> resolve,
                        ushort prefetchCount = 10);

                /// <summary>
                /// The subscriber node will receive the message by its own queue.
                /// </summary>
                /// <param name="handle"></param>
                /// <param name="prefetchCount"></param>
                /// <typeparam name="T"></typeparam>
                void ListenMessage<T>(Action<T> handle, ushort prefetchCount = 10);

                void RepublishDeadLetterEvent<T>(string deadLetterQueueName, ushort prefetchCount = 10);
        }
}