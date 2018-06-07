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
            _messageBus.ReceiveEvent<TestEvent>(TestEventExceptionHandler);
            //_messageBus.ReceiveEvent<TestEventWithVersion>(TestEventWithVersionHandler);
            _messageBus.ReceiveEvent<TestEventWithVersion>(TestEventExceptionWithVersionHandler);
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