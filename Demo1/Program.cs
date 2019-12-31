using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .Build()
                .Run();
    }
}