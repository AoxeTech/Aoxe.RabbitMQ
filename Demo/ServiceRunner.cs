using System;
using Zaabee.RabbitMQ.Abstractions;

namespace Demo
{
    public class ServiceRunner
    {
        private readonly IZaabeeRabbitMqClient _messageBus;

        public ServiceRunner(IZaabeeRabbitMqClient messageBus)
        {
            _messageBus = messageBus;
        }

        public void Start()
        {
            //_messageBus.ReceiveEvent<TestEvent>(TestEventHandler);
            _messageBus.ReceiveEvent<TestEvent>(TestEventExceptionHandler, 15);
            //_messageBus.ReceiveEvent<TestEventWithVersion>(TestEventWithVersionHandler);
            _messageBus.ReceiveEvent<TestEventWithVersion>(TestEventExceptionWithVersionHandler, 20);
            _messageBus.ReceiveMessage<TestMessage>(TestMessageHandler);
        }

        public void TestEventHandler(TestEvent testEvent)
        {

        }

        public void TestEventExceptionHandler(TestEvent testEvent)
        {
            throw new Exception("Test");
        }

        public void TestEventWithVersionHandler(TestEventWithVersion testEventWithVersion)
        {

        }

        public void TestEventExceptionWithVersionHandler(TestEventWithVersion testEventWithVersion)
        {
            throw new Exception("Test");
        }

        public void TestMessageHandler(TestMessage testMessage)
        {

        }
    }
}