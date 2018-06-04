using System.Collections.Generic;

namespace Zaabee.RabbitMQ
{
    /// <summary>
    /// 交换机参数
    /// </summary>
    internal class ExchangeParam
    {
        private string _exchange;

        /// <summary>
        /// 交换机名
        /// </summary>
        public string Exchange
        {
            get => _exchange?.Trim();
            set => _exchange = value;
        }

        /// <summary>
        /// 交换机类型（默认Fanout）
        /// </summary>
        public ExchangeType Type { get; set; } = ExchangeType.Fanout;

        /// <summary>
        /// 是否持久化（默认true）
        /// </summary>
        public bool Durable { get; set; } = true;

        /// <summary>
        /// 是否自动删除（默认false）
        /// </summary>
        public bool AutoDelete { get; set; } = false;

        /// <summary>
        /// 参数
        /// </summary>
        public IDictionary<string, object> Arguments { get; set; }
    }
}