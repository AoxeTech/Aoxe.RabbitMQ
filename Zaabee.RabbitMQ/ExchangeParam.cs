using System.Collections.Generic;

namespace Zaabee.RabbitMQ
{
    internal class ExchangeParam
    {
        private string _exchange;
        public string Exchange
        {
            get => _exchange?.Trim();
            set => _exchange = value;
        }

        public ExchangeType Type { get; set; } = ExchangeType.Fanout;

        public bool Durable { get; set; } = true;

        public bool AutoDelete { get; set; } = false;

        public IDictionary<string, object> Arguments { get; set; }
    }
}