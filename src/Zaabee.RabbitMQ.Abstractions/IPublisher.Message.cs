namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    void SendMessage<T>(T message);
    void SendMessage(string topic, byte[] body);
    void PublishMessage<T>(T message);
    void PublishMessage<T>(string topic, T message);
    void PublishMessage(string topic, byte[] body);
}