using System.Collections.Generic;

namespace Zaabee.RabbitMQ
{
    /// <summary>
    /// 队列参数
    /// </summary>
    internal class QueueParam
    {
        private string _queue = "UndefinedQueueName";

        /// <summary>
        /// 队列名
        /// </summary>
        public string Queue
        {
            get => _queue;
            set => _queue = value?.Trim() ?? "UndefinedQueueName";
        }

        /// <summary>
        /// 是否持久化（默认true）
        /// </summary>
        public bool Durable { get; set; } = true;

        /// <summary>
        /// 排他队列（默认false），如果一个队列被声明为排他队列，该队列仅对首次声明它的连接可见，
        /// 并在连接断开时自动删除。这里需要注意三点：其一，排他队列是基于连接可见的，同一连接的不同信道是可
        /// 以同时访问同一个连接创建的排他队列的。其二，“首次”，如果一个连接已经声明了一个排他队列，其他连
        /// 接是不允许建立同名的排他队列的，这个与普通队列不同。其三，即使该队列是持久化的，一旦连接关闭或者
        /// 客户端退出，该排他队列都会被自动删除的。这种队列适用于只限于一个客户端发送读取消息的应用场景。
        /// </summary>
        public bool Exclusive { get; set; }

        /// <summary>
        /// 是否自动删除
        /// </summary>
        public bool AutoDelete { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public IDictionary<string, object> Arguments { get; set; }
    }
}