namespace Zaabee.RabbitMQ;

internal class DeadLetterMsg
{
    public string QueueName { get; set; } = null!;
    public string ExMsg { get; set; } = null!;
    public string ExStack { get; set; } = null!;
    public DateTimeOffset ThrowTime { get; set; }
    public string BodyString { get; set; } = null!;
}