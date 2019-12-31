using System.Threading.Tasks;

namespace Zaabee.RabbitMQ.Abstractions
{
    public interface IPublisher
    {
        void PublishEvent<T>(T @event);
        void PublishEvent<T>(string exchangeName, T @event);
        void PublishEvent(string exchangeName, byte[] body);
        void PublishMessage<T>(T message);
        void PublishMessage<T>(string exchangeName, T message);
        void PublishMessage(string exchangeName, byte[] body);

        Task PublishEventAsync<T>(T @event);
        Task PublishEventAsync<T>(string exchangeName, T @event);
        Task PublishEventAsync(string exchangeName, byte[] body);
        Task PublishMessageAsync<T>(T message);
        Task PublishMessageAsync<T>(string exchangeName, T message);
        Task PublishMessageAsync(string exchangeName, byte[] body);
    }
}