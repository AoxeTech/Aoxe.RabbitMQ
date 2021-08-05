using RabbitMQ.Client;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
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
    }
}