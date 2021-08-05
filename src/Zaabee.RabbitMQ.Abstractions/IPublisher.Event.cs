namespace Zaabee.RabbitMQ.Abstractions
{
    public partial interface IPublisher
    {
        void PublishEvent<T>(T @event);
        void PublishEvent<T>(string exchangeName, T @event);
        void PublishEvent(string exchangeName, byte[] body);
    }
}