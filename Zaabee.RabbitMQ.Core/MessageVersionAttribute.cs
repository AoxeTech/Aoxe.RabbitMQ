using System;

namespace Zaabee.RabbitMQ.Core
{
    public class MessageVersionAttribute : Attribute
    {
        public MessageVersionAttribute(string version)
        {
            Version = version;
        }

        public string Version { get; }
    }
}