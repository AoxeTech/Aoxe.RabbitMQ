using System.Threading.Tasks;

namespace Zaabee.RabbitMQ.Abstractions
{
    public partial interface IPublisher
    {
        #region Event

        Task PublishEventAsync<T>(T @event);
        Task PublishEventAsync<T>(string exchangeName, T @event);
        Task PublishEventAsync(string exchangeName, byte[] body);

        #endregion

        #region Message

        Task PublishMessageAsync<T>(T message);
        Task PublishMessageAsync<T>(string exchangeName, T message);
        Task PublishMessageAsync(string exchangeName, byte[] body);

        #endregion

        #region Command

        Task PublishCommandAsync<T>(T command);
        Task PublishCommandAsync(string exchangeName, byte[] body);

        #endregion
    }
}