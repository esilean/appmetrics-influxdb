using App.Metrics;
using AppMetricsInfluxDB.Data.Data;
using AppMetricsInfluxDB.Data.Repositories;
using AppMetricsInfluxDB.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AppMetricsInfluxDB.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PromeDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            var metrics = new MetricsBuilder()
                .Report.ToInfluxDb(
                    options =>
                    {
                        options.InfluxDb.BaseUri = new Uri("http://localhost:8086");
                        options.InfluxDb.Database = "appmetrics_influxdb";
                        //options.InfluxDb.Consistenency = "consistency";
                        //options.InfluxDb.UserName = "admin";
                        //options.InfluxDb.Password = "password";
                        options.InfluxDb.CreateDataBaseIfNotExists = true;
                        //options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                        //options.HttpPolicy.FailuresBeforeBackoff = 5;
                        //options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                        //options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                        options.FlushInterval = TimeSpan.FromSeconds(20);
                    })
                .Build();
            services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsReportingHostedService();

            services.AddControllers()
                        .AddMetrics();


            services.AddScoped<IPromeRepository, PromeRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseMetricsAllMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
