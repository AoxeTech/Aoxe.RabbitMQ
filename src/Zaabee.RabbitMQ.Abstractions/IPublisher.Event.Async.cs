namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    Task SendEventAsync<T>(T message);
    Task SendEventAsync(string topic, byte[] body);
    Task PublishEventAsync<T>(T @event);
    Task PublishEventAsync<T>(string topic, T @event);
    Task PublishEventAsync(string topic, byte[] body);
}