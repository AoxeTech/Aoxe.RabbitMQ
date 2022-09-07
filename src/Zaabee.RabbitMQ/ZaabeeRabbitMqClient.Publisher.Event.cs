namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public void PublishEvent<T>(
        T @event) =>
        PublishMessage(@event, true);

    public void PublishEvent<T>(
        string topic,
        T @event) =>
        PublishMessage(topic, @event, true);

    public void PublishEvent(
        string topic,
        byte[] body) =>
        PublishMessage(topic, body, true);
}