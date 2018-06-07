using System;

namespace Zaabee.RabbitMQ.Abstractions
{
    public interface IZaabeeRabbitMqClient
    {
        void PublishEvent<T>(T @event);
        void PublishEvent(string eventName, byte[] body);
        void PublishMessage<T>(T message);
        void PublishMessage(string messageName, byte[] body);

        /// <summary>
        /// The subscriber cluster will receive the event by the default queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void ReceiveEvent<T>(Action<T> handle);

        /// <summary>
        /// The subscriber cluster will receive the event by its own queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void SubscribeEvent<T>(Action<T> handle);

        /// <summary>
        /// The subscriber cluster will receive the message by the default queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void ReceiveMessage<T>(Action<T> handle);

        /// <summary>
        /// The subscriber cluster will receive the message by its own queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void SubscribeMessage<T>(Action<T> handle);

        /// <summary>
        /// The subscriber node will receive the message by its own queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        void ListenMessage<T>(Action<T> handle);
    }
}