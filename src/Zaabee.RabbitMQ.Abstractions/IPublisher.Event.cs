namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    void SendEvent<T>(T @event);
    void SendEvent(string topic, byte[] body);
    void PublishEvent<T>(T @event);
    void PublishEvent<T>(string topic, T @event);
    void PublishEvent(string topic, byte[] body);
}