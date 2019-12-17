using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.RabbitMQ.NewtonsoftJson;

namespace Demo
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
            services.AddControllers();
            services.AddSingleton<IZaabeeRabbitMqClient, ZaabeeRabbitMqClient>(p =>
                new ZaabeeRabbitMqClient(new MqConfig
                {
                    AutomaticRecoveryEnabled = true,
                    HeartBeat = 60,
                    NetworkRecoveryInterval = new TimeSpan(60),
                    Hosts = new List<string>{"192.168.78.140"},
                    UserName = "guest",
                    Password = "guest"
                }, new Serializer()));
            services.AddSingleton<ServiceRunner>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ServiceRunner runner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            runner.Start();
        }
    }
}