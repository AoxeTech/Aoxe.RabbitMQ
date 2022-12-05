namespace Zaabee.RabbitMQ.Demo;

public static class Program
{
    public static void Main(string[] args) =>
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .Build()
            .Run();
}