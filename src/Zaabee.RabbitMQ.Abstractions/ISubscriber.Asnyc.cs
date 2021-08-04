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
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ReceiveEventAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ReceiveEventAsync<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeEventAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeEventAsync<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeEventAsync<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeEventAsync<T>(string queue, Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified exchange and queue.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeEventAsync<T>(string exchange, string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Event by the specified exchange and queue.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeEventAsync<T>(string exchange, string queue, Func<Func<T, Task>> resolve,
            ushort prefetchCount = 10);

        #endregion

        #region Message

        /// <summary>
        /// The subscriber cluster will receive the Message by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ReceiveMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ReceiveMessageAsync<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeMessageAsync<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeMessageAsync<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Message by the specified queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeMessageAsync<T>(string queue, Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeMessageAsync<T>(string exchange, string queue, Func<Action<T>> resolve,
            ushort prefetchCount = 10);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SubscribeMessageAsync<T>(string exchange, string queue, Func<Func<T, Task>> resolve,
            ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber node will receive the Message by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ListenMessageAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber node will receive the Message by its own queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ListenMessageAsync<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        #endregion

        #region Command

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ReceiveCommandAsync<T>(Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ReceiveCommandAsync<T>(Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ReceiveCommandAsync<T>(string queue, Func<Action<T>> resolve, ushort prefetchCount = 10);

        /// <summary>
        /// The subscriber cluster will receive the Command by the default queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="resolve"></param>
        /// <param name="prefetchCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task ReceiveCommandAsync<T>(string queue, Func<Func<T, Task>> resolve, ushort prefetchCount = 10);

        #endregion
    }
}