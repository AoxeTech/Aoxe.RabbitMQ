namespace Zaabee.RabbitMQ.Abstractions
{
    public partial interface IPublisher
    {
        void PublishMessage<T>(T message);
        void PublishMessage<T>(string exchangeName, T message);
        void PublishMessage(string exchangeName, byte[] body);
    }
}