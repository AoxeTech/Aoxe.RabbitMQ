using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        #region Event

        public void PublishEvent<T>(T @event) =>
            PublishEvent(GetTypeName(typeof(T)), @event);

        public void PublishEvent<T>(string exchangeName, T @event) =>
            PublishEvent(exchangeName, _serializer.Serialize(@event));

        public void PublishEvent(string exchangeName, ReadOnlyMemory<byte> body)
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

        public async Task PublishEventAsync<T>(T @event) =>
            await Task.Run(() => { PublishEvent(@event); });

        public async Task PublishEventAsync<T>(string exchangeName, T @event) =>
            await Task.Run(() => { PublishEvent(exchangeName, @event); });

        public async Task PublishEventAsync(string exchangeName, ReadOnlyMemory<byte> body) =>
            await Task.Run(() => { PublishEvent(exchangeName, body); });

        #endregion

        #region Message

        public void PublishMessage<T>(T message) =>
            PublishMessage(GetTypeName(typeof(T)), message);

        public void PublishMessage<T>(string exchangeName, T message) =>
            PublishMessage(exchangeName, _serializer.Serialize(message));

        public void PublishMessage(string exchangeName, ReadOnlyMemory<byte> body)
        {
            var exchangeParam = new ExchangeParam {Exchange = exchangeName, Durable = false};
            using (var channel = GetPublisherChannel(exchangeParam, null))
            {
                var routingKey = exchangeParam.Exchange;
                channel.BasicPublish(exchangeParam.Exchange, routingKey, null, body);
            }
        }

        public async Task PublishMessageAsync<T>(T message) =>
            await Task.Run(() => { PublishMessage(message); });

        public async Task PublishMessageAsync<T>(string exchangeName, T message) =>
            await Task.Run(() => { PublishMessage(exchangeName, message); });

        public async Task PublishMessageAsync(string exchangeName, ReadOnlyMemory<byte> body) =>
            await Task.Run(() => { PublishMessage(exchangeName, body); });

        #endregion

        #region Command

        public void PublishCommand<T>(T command) =>
            PublishCommand(GetTypeName(typeof(T)), _serializer.Serialize(command));

        public void PublishCommand(string exchangeName, ReadOnlyMemory<byte> body)
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

        public async Task PublishCommandAsync<T>(T command) =>
            await Task.Run(() => { PublishCommand(command); });

        public async Task PublishCommandAsync(string exchangeName, ReadOnlyMemory<byte> body) =>
            await Task.Run(() => { PublishCommand(exchangeName, body); });

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