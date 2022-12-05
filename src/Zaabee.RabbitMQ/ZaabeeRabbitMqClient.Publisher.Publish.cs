namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void PublishMessage<T>(
        T message,
        bool persistence = false)
    {
        var topic = GetTypeName(typeof(T));
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(exchangeParam, null, persistence, message);
    }

    public void PublishMessage<T>(
        string topic,
        T message,
        bool persistence = false)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(exchangeParam, null, persistence, message);
    }

    public void PublishMessage(
        string topic,
        byte[] body,
        bool persistence = false)
    {
        var exchangeParam = GetExchangeParam(topic, persistence);
        Publish(exchangeParam, null, persistence, body);
    }
}