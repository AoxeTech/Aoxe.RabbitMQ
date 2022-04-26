namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    Task SendMessageAsync<T>(T message);
    Task SendMessageAsync(string topic, byte[] body);
    Task PublishMessageAsync<T>(T message);
    Task PublishMessageAsync<T>(string topic, T message);
    Task PublishMessageAsync(string topic, byte[] body);
}