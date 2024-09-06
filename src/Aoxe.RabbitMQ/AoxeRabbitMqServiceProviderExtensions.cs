namespace Aoxe.RabbitMQ;

public static class AoxeRabbitMqServiceProviderExtensions
{
    public static IServiceCollection AddAoxeMongo(
        this IServiceCollection services,
        AoxeRabbitMqOptions options
    ) => services.AddSingleton<IAoxeRabbitMqClient>(new AoxeRabbitMqClient(options));
}
