using RabbitMQ.Client;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        public void PublishMessage<T>(T message) =>
            PublishMessage(GetTypeName(typeof(T)), message);

        public void PublishMessage<T>(string exchangeName, T message) =>
            PublishMessage(exchangeName, _serializer.SerializeToBytes(message));

        public void PublishMessage(string exchangeName, byte[] body)
        {
            var exchangeParam = new ExchangeParam { Exchange = exchangeName, Durable = false };
            using (var channel = GetPublisherChannel(exchangeParam, null))
            {
                var routingKey = exchangeParam.Exchange;
                channel.BasicPublish(exchangeParam.Exchange, routingKey, null, body);
            }
        }
    }
}