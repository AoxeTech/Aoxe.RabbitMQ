using System;

namespace Zaabee.RabbitMQ
{
    internal class DeadLetterMsg
    {
        public string QueueName { get; set; }
        public string ExMsg { get; set; }
        public string ExStack { get; set; }
        public DateTimeOffset ThrowTime { get; set; }
        public string BodyString { get; set; }
    }
}