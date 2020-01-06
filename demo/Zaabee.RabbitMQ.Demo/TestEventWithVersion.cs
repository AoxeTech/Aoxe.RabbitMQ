﻿using System;
using Zaabee.RabbitMQ.Abstractions;

namespace Zaabee.RabbitMQ.Demo
{
    [MessageVersion("3.14")]
    public class TestEventWithVersion
    {
        public Guid Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}