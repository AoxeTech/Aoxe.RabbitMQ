using System.Threading.Tasks;

namespace Zaabee.RabbitMQ.Abstractions
{
    public partial interface IPublisher
    {
        Task PublishMessageAsync<T>(T message);
        Task PublishMessageAsync<T>(string exchangeName, T message);
        Task PublishMessageAsync(string exchangeName, byte[] body);
    }
}