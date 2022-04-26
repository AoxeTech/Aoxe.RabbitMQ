namespace Zaabee.RabbitMQ.Abstractions;

public partial interface IPublisher
{
    void SendMessage<T>(T message);
    void SendMessage(string exchangeName, byte[] body);
    void PublishMessage<T>(T message);
    void PublishMessage<T>(string exchangeName, T message);
    void PublishMessage(string exchangeName, byte[] body);
}