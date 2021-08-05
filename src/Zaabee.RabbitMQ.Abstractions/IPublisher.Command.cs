namespace Zaabee.RabbitMQ.Abstractions
{
    public partial interface IPublisher
    {
        void PublishCommand<T>(T command);
        void PublishCommand(string exchangeName, byte[] body);
    }
}