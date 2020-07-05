using System;
using System.Threading.Tasks;

namespace Zaabee.RabbitMQ.Abstractions
{
    public interface IPublisher
    {
        #region Event

        void PublishEvent<T>(T @event);
        void PublishEvent<T>(string exchangeName, T @event);
        void PublishEvent(string exchangeName, ReadOnlyMemory<byte> body);
        Task PublishEventAsync<T>(T @event);
        Task PublishEventAsync<T>(string exchangeName, T @event);
        Task PublishEventAsync(string exchangeName, ReadOnlyMemory<byte> body);

        #endregion

        #region Message

        void PublishMessage<T>(T message);
        void PublishMessage<T>(string exchangeName, T message);
        void PublishMessage(string exchangeName, ReadOnlyMemory<byte> body);
        Task PublishMessageAsync<T>(T message);
        Task PublishMessageAsync<T>(string exchangeName, T message);
        Task PublishMessageAsync(string exchangeName, ReadOnlyMemory<byte> body);

        #endregion

        #region Command

        void PublishCommand<T>(T command);
        void PublishCommand(string exchangeName, ReadOnlyMemory<byte> body);
        Task PublishCommandAsync<T>(T command);
        Task PublishCommandAsync(string exchangeName, ReadOnlyMemory<byte> body);

        #endregion
    }
}