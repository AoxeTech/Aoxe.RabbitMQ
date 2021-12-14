namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    Task PublishEventAsync<T>(T @event);
    Task PublishEventAsync<T>(string exchangeName, T @event);
    Task PublishEventAsync(string exchangeName, byte[] body);
}