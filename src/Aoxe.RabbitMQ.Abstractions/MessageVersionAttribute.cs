namespace Aoxe.RabbitMQ.Abstractions;

public class MessageVersionAttribute : Attribute
{
    public MessageVersionAttribute(string version)
    {
        Version = version;
    }

    public string Version { get; }
}
