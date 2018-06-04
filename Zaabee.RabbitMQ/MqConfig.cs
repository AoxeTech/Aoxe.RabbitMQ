using System;
using System.Collections.Generic;

namespace Zaabee.RabbitMQ
{
    /// <summary>
    /// MQ配置类
    /// </summary>
    public class MqConfig
    {
        private List<string> _hosts;

        public List<string> Hosts
        {
            get => _hosts ?? (_hosts = new List<string>());
            set => _hosts = value;
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        private ushort _heartBeat;

        public ushort HeartBeat
        {
            get => _heartBeat == 0 ? (_heartBeat = 60) : _heartBeat;
            set => _heartBeat = value;
        }

        public bool AutomaticRecoveryEnabled { get; set; } = true;

        private TimeSpan _networkRecoveryInterval;

        public TimeSpan NetworkRecoveryInterval
        {
            get => _networkRecoveryInterval.Ticks == 0
                ? (_networkRecoveryInterval = new TimeSpan(60))
                : _networkRecoveryInterval;
            set => _networkRecoveryInterval = value;
        }

        private string _virtualHost;

        public string VirtualHost
        {
            get => _virtualHost ?? (_virtualHost = "");
            set => _virtualHost = value;
        }
    }
}