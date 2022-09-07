namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public Task SendMessageAsync<T>(
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true) =>
        Task.Run(() => { SendMessage(message); });

    public Task SendMessageAsync<T>(
        string topic,
        T message,
        bool persistence = false,
        int retry = 3,
        bool dlx = true) =>
        Task.Run(() => { SendMessage(topic, message); });

    public Task SendMessageAsync(
        string topic,
        byte[] body,
        bool persistence = false,
        int retry = 3,
        bool dlx = true) =>
        Task.Run(() => { SendMessage(topic, body); });
}