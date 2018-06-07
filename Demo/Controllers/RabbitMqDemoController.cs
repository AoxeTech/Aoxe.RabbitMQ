using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Zaabee.RabbitMQ.Abstractions;

namespace Demo.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RabbitMqDemoController : Controller
    {
        private readonly IZaabeeRabbitMqClient _messageBus;

        public RabbitMqDemoController(IZaabeeRabbitMqClient messageBus)
        {
            _messageBus = messageBus;
        }

        [HttpGet]
        [HttpPost]
        public long PublishEvent(int quantity)
        {
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < quantity; i++)
            {
                _messageBus.PublishEvent(new TestEvent
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTimeOffset.Now
                });
            }

            return sw.ElapsedMilliseconds;
        }

        [HttpGet]
        [HttpPost]
        public long PublishEventWithVersion(int quantity)
        {
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < quantity; i++)
            {
                _messageBus.PublishEvent(new TestEventWithVersion
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTimeOffset.Now
                });
            }

            return sw.ElapsedMilliseconds;
        }

        [HttpGet]
        [HttpPost]
        public long PublishMessage(int quantity)
        {
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < quantity; i++)
            {
                _messageBus.PublishMessage(new TestMessage
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTimeOffset.Now
                });
            }

            return sw.ElapsedMilliseconds;
        }
    }
}