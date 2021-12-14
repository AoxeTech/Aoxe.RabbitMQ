namespace Zaabee.RabbitMQ;

public partial class ZaabeeRabbitMqClient
{
    public async Task PublishCommandAsync<T>(T command) =>
        await Task.Run(() => { PublishCommand(command); });

    public async Task PublishCommandAsync(string exchangeName, byte[] body) =>
        await Task.Run(() => { PublishCommand(exchangeName, body); });
}