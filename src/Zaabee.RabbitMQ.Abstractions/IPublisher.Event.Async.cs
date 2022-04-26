namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    Task SendEventAsync<T>(T message);
    Task SendEventAsync(string exchangeName, byte[] body);
    Task PublishEventAsync<T>(T @event);
    Task PublishEventAsync<T>(string exchangeName, T @event);
    Task PublishEventAsync(string exchangeName, byte[] body);
}