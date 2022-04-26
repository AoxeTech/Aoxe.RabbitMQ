namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    Task SendMessageAsync<T>(T message);
    Task SendMessageAsync(string exchangeName, byte[] body);
    Task PublishMessageAsync<T>(T message);
    Task PublishMessageAsync<T>(string exchangeName, T message);
    Task PublishMessageAsync(string exchangeName, byte[] body);
}