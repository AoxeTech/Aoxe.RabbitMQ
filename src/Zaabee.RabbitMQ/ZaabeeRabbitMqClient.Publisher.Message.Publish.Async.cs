namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task PublishMessageAsync<T>(
        T message,
        bool persistence = false) =>
        Task.Run(() => { PublishMessage(message); });

    public Task PublishMessageAsync<T>(
        string topic,
        T message,
        bool persistence = false) =>
        Task.Run(() => { PublishMessage(topic, message); });

    public Task PublishMessageAsync(
        string topic,
        byte[] body,
        bool persistence = false) =>
        Task.Run(() => { PublishMessage(topic, body); });
}