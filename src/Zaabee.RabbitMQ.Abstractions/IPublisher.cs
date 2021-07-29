namespace Zaabee.RabbitMQ.Abstractions
{
    public partial interface IPublisher
    {
        #region Event

        void PublishEvent<T>(T @event);
        void PublishEvent<T>(string exchangeName, T @event);
        void PublishEvent(string exchangeName, byte[] body);

        #endregion

        #region Message

        void PublishMessage<T>(T message);
        void PublishMessage<T>(string exchangeName, T message);
        void PublishMessage(string exchangeName, byte[] body);

        #endregion

        #region Command

        void PublishCommand<T>(T command);
        void PublishCommand(string exchangeName, byte[] body);

        #endregion
    }
}