namespace Aoxe.RabbitMQ.Internal;

internal class ExchangeParam
{
    private string _exchange = "UndefinedExchangeName";

    public string Exchange
    {
        get => _exchange;
        set => _exchange = string.IsNullOrWhiteSpace(value) ? _exchange : value.Trim();
    }

    public ExchangeType Type { get; set; } = ExchangeType.Fanout;

    public bool Durable { get; set; } = true;

    public bool AutoDelete { get; set; } = false;

    public IDictionary<string, object>? Arguments { get; set; }
}
