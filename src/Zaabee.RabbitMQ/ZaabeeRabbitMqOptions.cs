namespace Zaabee.RabbitMQ;

public class ZaabeeRabbitMqOptions
{
    public List<string> Hosts { get; set; } = new();

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    private TimeSpan _heartBeat = TimeSpan.FromMinutes(1.0);

    public TimeSpan HeartBeat
    {
        get => _heartBeat;
        set => _heartBeat = value == TimeSpan.Zero ? _heartBeat : value;
    }

    public bool AutomaticRecoveryEnabled { get; set; } = true;

    private TimeSpan _networkRecoveryInterval = TimeSpan.FromSeconds(5.0);

    public TimeSpan NetworkRecoveryInterval
    {
        get => _networkRecoveryInterval;
        set => _networkRecoveryInterval = value == TimeSpan.Zero ? _networkRecoveryInterval : value;
    }

    private string _virtualHost = string.Empty;

    public string VirtualHost
    {
        get => _virtualHost;
        set => _virtualHost = string.IsNullOrWhiteSpace(value) ? _virtualHost : value;
    }

    public IJsonSerializer Serializer { get; set; } = null!;
}