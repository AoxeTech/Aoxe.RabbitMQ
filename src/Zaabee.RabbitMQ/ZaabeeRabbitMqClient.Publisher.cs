using RabbitMQ.Client;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        #region Event

        public void PublishEvent<T>(T @event) =>
            PublishEvent(GetTypeName(typeof(T)), @event);

        public void PublishEvent<T>(string exchangeName, T @event) =>
            PublishEvent(exchangeName, _serializer.SerializeToBytes(@event));

        public void PublishEvent(string exchangeName, byte[] body)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchangeName};
            using (var channel = GetPublisherChannel(exchangeParam, null))
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                var routingKey = exchangeParam.Exchange;

                channel.BasicPublish(exchangeParam.Exchange, routingKey, properties, body);
            }
        }

        #endregion

        #region Message

        public void PublishMessage<T>(T message) =>
            PublishMessage(GetTypeName(typeof(T)), message);

        public void PublishMessage<T>(string exchangeName, T message) =>
            PublishMessage(exchangeName, _serializer.SerializeToBytes(message));

        public void PublishMessage(string exchangeName, byte[] body)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchangeName, Durable = false};
            using (var channel = GetPublisherChannel(exchangeParam, null))
            {
                var routingKey = exchangeParam.Exchange;
                channel.BasicPublish(exchangeParam.Exchange, routingKey, null, body);
            }
        }

        #endregion

        #region Command

        public void PublishCommand<T>(T command) =>
            PublishCommand(GetTypeName(typeof(T)), _serializer.SerializeToBytes(command));

        public void PublishCommand(string exchangeName, byte[] body)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchangeName};
            var queueParam = new QueueParam {Queue = exchangeName};
            using (var channel = GetPublisherChannel(exchangeParam, queueParam))
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                var routingKey = exchangeParam.Exchange;

                channel.BasicPublish(exchangeParam.Exchange, routingKey, properties, body);
            }
        }

        #endregion

        private IModel GetPublisherChannel(ExchangeParam exchangeParam, QueueParam queueParam, string routingKey = null)
        {
            var channel = _publishConn.CreateModel();

            channel.ExchangeDeclare(exchange: exchangeParam.Exchange, type: exchangeParam.Type.ToString().ToLower(),
                durable: exchangeParam.Durable, autoDelete: exchangeParam.AutoDelete,
                arguments: exchangeParam.Arguments);

            if (queueParam is null) return channel;

            channel.QueueDeclare(queue: queueParam.Queue, durable: queueParam.Durable, exclusive: queueParam.Exclusive,
                autoDelete: queueParam.AutoDelete, arguments: queueParam.Arguments);
            channel.QueueBind(queue: queueParam.Queue, exchange: exchangeParam.Exchange,
                routingKey: routingKey ?? queueParam.Queue);

            return channel;
        }
    }
}