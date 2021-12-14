namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    Task PublishCommandAsync<T>(T command);
    Task PublishCommandAsync(string exchangeName, byte[] body);
}