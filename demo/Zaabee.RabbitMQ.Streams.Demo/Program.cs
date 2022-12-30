// See https://aka.ms/new-console-template for more information

var config = new StreamSystemConfig
{
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/"
};
// Connect to the broker and create the system object
// the entry point for the client.
// Create it once and reuse it.
var system = await StreamSystem.Create(config);

const string stream = "my_first_stream";

// Create the stream. It is important to put some retention policy 
// in this case is 200000 bytes.
await system.CreateStream(new StreamSpec(stream)
{
    MaxLengthBytes = 200000,
});

var producer = await Producer.Create(
    new ProducerConfig(system, stream)
    {
        Reference = Guid.NewGuid().ToString(),


        // Receive the confirmation of the messages sent
        ConfirmationHandler = confirmation =>
        {
            switch (confirmation.Status)
            {
                // ConfirmationStatus.Confirmed: The message was successfully sent
                case ConfirmationStatus.Confirmed:
                    Console.WriteLine($"Message {confirmation.PublishingId} confirmed");
                    break;
                // There is an error during the sending of the message
                case ConfirmationStatus.WaitForConfirmation:
                case ConfirmationStatus.ClientTimeoutError
                    : // The client didn't receive the confirmation in time. 
                // but it doesn't mean that the message was not sent
                // maybe the broker needs more time to confirm the message
                // see TimeoutMessageAfter in the ProducerConfig
                case ConfirmationStatus.StreamNotAvailable:
                case ConfirmationStatus.InternalError:
                case ConfirmationStatus.AccessRefused:
                case ConfirmationStatus.PreconditionFailed:
                case ConfirmationStatus.PublisherDoesNotExist:
                case ConfirmationStatus.UndefinedError:
                default:
                    Console.WriteLine(
                        $"Message  {confirmation.PublishingId} not confirmed. Error {confirmation.Status}");
                    break;
            }

            return Task.CompletedTask;
        }
    });

// Publish the messages
for (var i = 0; i < 100; i++)
{
    var message = new Message(Encoding.UTF8.GetBytes($"hello {i}"));
    await producer.Send(message);
}

// not mandatory. Just to show the confirmation
Thread.Sleep(TimeSpan.FromSeconds(1));

// Create a consumer
var consumer = await Consumer.Create(
    new ConsumerConfig(system, stream)
    {
        Reference = "my_consumer",
        // Consume the stream from the beginning 
        // See also other OffsetSpec 
        OffsetSpec = new OffsetTypeFirst(),
        // Receive the messages
        MessageHandler = async (sourceStream, consumer, ctx, message) =>
        {
            Console.WriteLine(
                $"message: coming from {sourceStream} data: {Encoding.Default.GetString(message.Data.Contents.ToArray())} - consumed");
            await Task.CompletedTask;
        }
    });
Console.WriteLine("Press to stop");
Console.ReadLine();

await producer.Close();
await consumer.Close();
await system.DeleteStream(stream);
await system.Close();
