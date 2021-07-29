using System;
using System.Threading.Tasks;

namespace Zaabee.RabbitMQ.Abstractions
{
    public partial interface ISubscriber
    {
        #region Event

        /// <summary>
        /// The subscriber cluster will receive the Event by the default queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ReceiveEventAsync<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ReceiveEventAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by its own queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeEventAsync<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeEventAsync<T>(string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified exchange and queue.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeEventAsync<T>(string exchange, string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeEventAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeEventAsync<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified exchange and queue.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeEventAsync<T>(string exchange, string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        #endregion

        #region Message

        /// <summary>
        /// The subscriber cluster will receive the Message by the default queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ReceiveMessageAsync<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ReceiveMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by its own queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeMessageAsync<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeMessageAsync<T>(string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the specified exchange and queue.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeMessageAsync<T>(string exchange, string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeMessageAsync<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task SubscribeMessageAsync<T>(string exchange, string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber node will receive the Message by its own queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ListenMessageAsync<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber node will receive the Message by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ListenMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        #endregion

        #region Command

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ReceiveCommandAsync<T>(Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ReceiveCommandAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handle"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ReceiveCommandAsync<T>(string queue, Action<T> handle, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        Task ReceiveCommandAsync<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        #endregion
    }
}