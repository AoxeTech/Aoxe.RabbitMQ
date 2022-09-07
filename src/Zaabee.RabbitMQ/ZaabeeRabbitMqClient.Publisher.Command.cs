namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void SendCommand<T>(
        T command,
        int retry = 3,
        bool dlx = true)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(topic, MessageType.Event);
        Publish(exchangeParam, queueParam, MessageType.Event, command);
    }

    public void SendCommand<T>(
        string topic,
        T command,
        int retry = 3,
        bool dlx = true)
    {
        
    }

    public void SendCommand(
        string topic,
        byte[] body,
        int retry = 3,
        bool dlx = true)
    {
        var exchangeParam = GetExchangeParam(topic, MessageType.Event);
        var queueParam = GetQueueParam(topic, MessageType.Event);
        Publish(exchangeParam, queueParam, MessageType.Event, body);
    }
}