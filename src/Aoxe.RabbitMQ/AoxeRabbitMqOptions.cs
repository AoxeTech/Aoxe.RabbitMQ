namespace Aoxe.RabbitMQ;

public sealed class AoxeRabbitMqOptions
{
    public List<string> Hosts { get; set; } = new();
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool AutomaticRecoveryEnabled { get; set; } = true;
    public bool TopologyRecoveryEnabled { get; set; } = true;
    public TimeSpan HeartBeat { get; set; } = TimeSpan.FromSeconds(60.0);
    public TimeSpan NetworkRecoveryInterval { get; set; } = TimeSpan.FromSeconds(5.0);
    public TimeSpan RequestedConnectionTimeout { get; set; } = TimeSpan.FromSeconds(30.0);
    public TimeSpan SocketReadTimeout { get; set; } = TimeSpan.FromSeconds(30);
    public TimeSpan SocketWriteTimeout { get; set; } = TimeSpan.FromSeconds(30);
    public int ConsumerDispatchConcurrency { get; set; } = Consts.DefaultPrefetchCount;

    private string _virtualHost = string.Empty;

    public string VirtualHost
    {
        get => _virtualHost;
        set => _virtualHost = string.IsNullOrWhiteSpace(value) ? _virtualHost : value;
    }

    public IJsonSerializer Serializer { get; set; } = null!;
}
