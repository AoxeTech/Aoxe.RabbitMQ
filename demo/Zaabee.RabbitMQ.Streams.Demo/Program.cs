using System.Net;

var streamSystem = await StreamSystem.Create( // (1)
    new StreamSystemConfig() // (2)
    {
        UserName = "guest",
        Password = "guest",
        Endpoints = new List<EndPoint>() { new IPEndPoint(IPAddress.Loopback, 5552) }
    }
    // streamLogger // (3)
).ConfigureAwait(false);

// Create a stream

const string streamName = "my-stream";
await streamSystem.CreateStream(
    new StreamSpec(streamName) // (4)
    {
        MaxSegmentSizeBytes = 20_000_000 // (5)
    }).ConfigureAwait(false);

var confirmationTaskCompletionSource = new TaskCompletionSource<int>();
var confirmationCount = 0;
const int messageCount = 100;
var producer = await Producer.Create( // (1)
        new ProducerConfig(streamSystem, streamName)
        {
            ConfirmationHandler = async confirmation => // (2)
            {
                Interlocked.Increment(ref confirmationCount);

                // here you can handle the confirmation
                switch (confirmation.Status)
                {
                    case ConfirmationStatus.Confirmed: // (3)
                        // all the messages received here are confirmed
                        if (confirmationCount == messageCount)
                        {
                            Console.WriteLine("*********************************");
                            Console.WriteLine($"All the {messageCount} messages are confirmed");
                            Console.WriteLine("*********************************");
                        }

                        break;

                    case ConfirmationStatus.StreamNotAvailable:
                    case ConfirmationStatus.InternalError:
                    case ConfirmationStatus.AccessRefused:
                    case ConfirmationStatus.PreconditionFailed:
                    case ConfirmationStatus.PublisherDoesNotExist:
                    case ConfirmationStatus.UndefinedError:
                    case ConfirmationStatus.ClientTimeoutError:
                        // (4)
                        Console.WriteLine(
                            $"Message {confirmation.PublishingId} failed with {confirmation.Status}");
                        break;
                    case ConfirmationStatus.WaitForConfirmation:
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (confirmationCount == messageCount)
                {
                    confirmationTaskCompletionSource.SetResult(messageCount);
                }

                await Task.CompletedTask.ConfigureAwait(false);
            }
        }
        // producerLogger // (5)
    )
    .ConfigureAwait(false);


// Send 100 messages
Console.WriteLine("Starting publishing...");
for (var i = 0; i < messageCount; i++)
{
    await producer.Send( // (6)
        new Message(Encoding.ASCII.GetBytes($"{i}"))
    ).ConfigureAwait(false);
}


confirmationTaskCompletionSource.Task.Wait(); // (7)
await producer.Close().ConfigureAwait(false); // (8)

Console.WriteLine("Starting consuming...");
// var consumer = await Consumer.Create( // (1)
//         new ConsumerConfig(streamSystem, streamName)
//         {
//             OffsetSpec = new OffsetTypeFirst(), // (2)
//             MessageHandler = async (sourceStream, consumer, messageContext, message) => // (3)
//             {
//                 if (Interlocked.Increment(ref consumerCount) == MessageCount)
//                 {
//                     Console.WriteLine("*********************************");
//                     Console.WriteLine($"All the {MessageCount} messages are received");
//                     Console.WriteLine("*********************************");
//                     consumerTaskCompletionSource.SetResult(MessageCount);
//                 }
//                 await Task.CompletedTask.ConfigureAwait(false);
//             }
//         },
//         consumerLogger // (4)
//     )
//     .ConfigureAwait(false);
// consumerTaskCompletionSource.Task.Wait(); // (5)
// await consumer.Close().ConfigureAwait(false); // (6)