using System;

namespace Demo
{
    public class Subscriber
    {

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