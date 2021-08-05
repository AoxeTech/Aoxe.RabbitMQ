using RabbitMQ.Client;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
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
    }
}