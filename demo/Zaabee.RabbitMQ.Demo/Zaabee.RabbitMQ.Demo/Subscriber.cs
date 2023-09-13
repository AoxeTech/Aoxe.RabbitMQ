using Zaabee.NewtonsoftJson;

namespace Zaabee.RabbitMQ.Demo;

public class Subscriber
{
    public void TestEventHandler(TestEvent? testEvent)
    {
        Console.WriteLine($"{nameof(TestEventHandler)} has been triggered. {nameof(testEvent)}: {testEvent?.ToJson()}");
    }

    public Task TestEventHandlerAsync(TestEvent? testEvent)
    {
        Console.WriteLine($"{nameof(TestEventHandlerAsync)} has been triggered. {nameof(testEvent)}: {testEvent?.ToJson()}");
        return Task.CompletedTask;
    }

    public void TestEventExceptionHandler(TestEvent testEvent)
    {
        Console.WriteLine($"{nameof(TestEventExceptionHandler)} has been triggered. {nameof(testEvent)}: {testEvent.ToJson()}");
        throw new Exception("Test");
    }

    public void TestEventWithVersionHandler(TestEventWithVersion testEventWithVersion)
    {
        Console.WriteLine($"{nameof(TestEventWithVersionHandler)} has been triggered. {nameof(testEventWithVersion)}: {testEventWithVersion.ToJson()}");
    }

    public void TestEventExceptionWithVersionHandler(TestEventWithVersion? testEventWithVersion)
    {
        Console.WriteLine($"{nameof(TestEventExceptionWithVersionHandler)} has been triggered. {nameof(testEventWithVersion)}: {testEventWithVersion?.ToJson()}");
        throw new Exception("Test");
    }

    public void TestMessageHandler(TestMessage? testMessage)
    {
        var i = testMessage;
        if (i is not null)
            i.Timestamp = DateTimeOffset.Now;
    }
}