using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsulDiscovery.HttpClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WeatherForecastService
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

            // ע�� ConsulDiscovery �������
            services.AddConsulDiscovery(Configuration);
            // ���� SayHelloService ��HttpClient
            services.AddHttpClient("SayHelloService", c =>
                {
                    c.BaseAddress = new Uri("http://SayHelloService");
                })
                .AddHttpMessageHandler<DiscoveryHttpMessageHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // ���� ConsulDiscovery
            app.StartConsulDiscovery(lifetime);
        }
    }
}
