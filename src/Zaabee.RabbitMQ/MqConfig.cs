using System;
using System.Collections.Generic;

namespace Zaabee.RabbitMQ
{
    public class MqConfig
    {
        private List<string> _hosts = new List<string>();

        public List<string> Hosts
        {
            get => _hosts;
            set => _hosts = value ?? _hosts;
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        private TimeSpan _heartBeat = TimeSpan.FromMinutes(1);

        public TimeSpan HeartBeat
        {
            get => _heartBeat;
            set => _heartBeat = value == TimeSpan.Zero ? _heartBeat : value;
        }

        public bool AutomaticRecoveryEnabled { get; set; } = true;

        private TimeSpan _networkRecoveryInterval = new TimeSpan(60);

        public TimeSpan NetworkRecoveryInterval
        {
            get => _networkRecoveryInterval;
            set => _networkRecoveryInterval = value.Ticks is 0L ? _networkRecoveryInterval : value;
        }

        private string _virtualHost = string.Empty;

        public string VirtualHost
        {
            get => _virtualHost;
            set => _virtualHost = string.IsNullOrWhiteSpace(value) ? _virtualHost : value;
        }
    }
}