namespace Aoxe.RabbitMQ;

public static class AoxeRabbitMqServiceProviderExtensions
{
    public static IServiceCollection AddAoxeRabbitMq(
        this IServiceCollection services,
        Func<AoxeRabbitMqOptions> optionsFactory
    ) => services.AddSingleton<IAoxeRabbitMqClient>(new AoxeRabbitMqClient(optionsFactory));

    public static IServiceCollection AddAoxeRabbitMq(
        this IServiceCollection services,
        AoxeRabbitMqOptions options
    ) => services.AddSingleton<IAoxeRabbitMqClient>(new AoxeRabbitMqClient(options));
}
