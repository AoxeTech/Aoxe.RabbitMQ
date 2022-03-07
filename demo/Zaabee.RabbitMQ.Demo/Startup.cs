using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zaabee.RabbitMQ.Abstractions;

namespace Zaabee.RabbitMQ.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerDocument();
            services.AddControllers();
            services.AddSingleton<IZaabeeRabbitMqClient>(_ =>
                new ZaabeeRabbitMqClient(new ZaabeeRabbitMqOptions
                {
                    AutomaticRecoveryEnabled = true,
                    Hosts = new List<string> { "192.168.78.150" },
                    UserName = "admin",
                    Password = "123",
                    Serializer = new NewtonsoftJson.Serializer()
                }));
            services.AddHostedService<RabbitMqBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}