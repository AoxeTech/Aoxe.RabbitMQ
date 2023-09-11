namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void Publish<T>(
        T message,
        bool persistence = false)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(message, exchangeParam, null, persistence);
    }

    public void Publish<T>(
        string topic,
        T message,
        bool persistence = false)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(message, exchangeParam, null, persistence);
    }

    public void Publish(
        string topic,
        byte[] body,
        bool persistence = false)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(body, exchangeParam, null, persistence);
    }
}