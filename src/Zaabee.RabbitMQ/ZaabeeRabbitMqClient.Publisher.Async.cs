using System.Threading.Tasks;

namespace Zaabee.RabbitMQ
{
    public partial class ZaabeeRabbitMqClient
    {
        #region Event

        public async Task PublishEventAsync<T>(T @event) =>
            await Task.Run(() => { PublishEvent(@event); });

        public async Task PublishEventAsync<T>(string exchangeName, T @event) =>
            await Task.Run(() => { PublishEvent(exchangeName, @event); });

        public async Task PublishEventAsync(string exchangeName, byte[] body) =>
            await Task.Run(() => { PublishEvent(exchangeName, body); });

        #endregion

        #region Message

        public async Task PublishMessageAsync<T>(T message) =>
            await Task.Run(() => { PublishMessage(message); });

        public async Task PublishMessageAsync<T>(string exchangeName, T message) =>
            await Task.Run(() => { PublishMessage(exchangeName, message); });

        public async Task PublishMessageAsync(string exchangeName, byte[] body) =>
            await Task.Run(() => { PublishMessage(exchangeName, body); });

        #endregion

        #region Command

        public async Task PublishCommandAsync<T>(T command) =>
            await Task.Run(() => { PublishCommand(command); });

        public async Task PublishCommandAsync(string exchangeName, byte[] body) =>
            await Task.Run(() => { PublishCommand(exchangeName, body); });

        #endregion
        
    }
}