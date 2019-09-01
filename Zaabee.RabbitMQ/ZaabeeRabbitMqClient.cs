using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.RabbitMQ.ISerialize;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient : IZaabeeRabbitMqClient
    {
        private readonly IConnection _conn;
        private readonly ISerializer _serializer;

        private readonly ConcurrentDictionary<Type, string> _queueNameDic =
            new ConcurrentDictionary<Type, string>();

        public ZaabeeRabbitMqClient(MqConfig config, ISerializer serializer)
        {
            if (config is null) throw new ArgumentNullException(nameof(config));
            if (serializer is null) throw new ArgumentNullException(nameof(serializer));
            if (config.Hosts.Count == 0) throw new ArgumentNullException(nameof(config.Hosts));

            var factory = new ConnectionFactory
            {
                RequestedHeartbeat = config.HeartBeat,
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                NetworkRecoveryInterval = config.NetworkRecoveryInterval,
                UserName = config.UserName,
                Password = config.Password,
                VirtualHost = string.IsNullOrWhiteSpace(config.VirtualHost) ? "/" : config.VirtualHost,
            };

            _conn = config.Hosts.Any() ? factory.CreateConnection(config.Hosts) : factory.CreateConnection();
            _serializer = serializer;
        }

        public ZaabeeRabbitMqClient(IConnectionFactory connectionFactory, ISerializer serializer,
            IList<string> hosts = null)
        {
            if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));
            if (serializer is null) throw new ArgumentNullException(nameof(serializer));

            _conn = hosts != null && hosts.Any()
                ? connectionFactory.CreateConnection(hosts)
                : connectionFactory.CreateConnection();
            _serializer = serializer;
        }

        public ZaabeeRabbitMqClient(IConnection connection, ISerializer serializer)
        {
            _conn = connection ?? throw new ArgumentNullException(nameof(connection));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public void RepublishDeadLetterEvent<T>(string deadLetterQueueName, ushort prefetchCount = 1)
        {
            var queueParam = new QueueParam {Queue = deadLetterQueueName};
            var channel = GetReceiverChannel(null, queueParam, prefetchCount);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var msg = _serializer.Deserialize<DeadLetterMsg>(body);

                var republishExchangeParam =
                    new ExchangeParam {Exchange = $"republish-{deadLetterQueueName}", Durable = true};
                var republishQueueParam =
                    new QueueParam {Queue = FromDeadLetterName(deadLetterQueueName), Durable = true};
                using (var republishChannel = GetPublisherChannel(republishExchangeParam, republishQueueParam))
                {
                    var properties = republishChannel.CreateBasicProperties();
                    properties.Persistent = true;
                    var routingKey = republishExchangeParam.Exchange;

                    var deadLetter = _serializer.FromString<T>(msg.BodyString);

                    republishChannel.BasicPublish(republishExchangeParam.Exchange, routingKey, properties,
                        _serializer.Serialize(deadLetter));
                }

                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queue: deadLetterQueueName, autoAck: false, consumer: consumer);
        }

        private string GetTypeName(Type type)
        {
            return _queueNameDic.GetOrAdd(type,
                key => !(type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault() is
                    MessageVersionAttribute msgVerAttr)
                    ? type.ToString()
                    : $"{type}[{msgVerAttr.Version}]");
        }

        private string GetQueueName<T>(Action<T> handle)
        {
            var messageName = GetTypeName(typeof(T));
            return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]";
        }

        private static string GetDeadLetterName(string name)=>
            $"dead-letter-{name}";

        private static string FromDeadLetterName(string deadLetterName)=>
            deadLetterName.Replace("dead-letter-", "");

        public void Dispose()
        {
            foreach (var keyValuePair in _subscriberChannelDic)
                keyValuePair.Value.Dispose();

            _conn.Dispose();
        }
    }
}