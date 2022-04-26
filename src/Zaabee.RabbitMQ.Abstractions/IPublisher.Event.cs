namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    void SendEvent<T>(T @event);
    void SendEvent(string exchangeName, byte[] body);
    void PublishEvent<T>(T @event);
    void PublishEvent<T>(string exchangeName, T @event);
    void PublishEvent(string exchangeName, byte[] body);
}