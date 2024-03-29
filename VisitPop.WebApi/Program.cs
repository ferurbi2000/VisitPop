using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Reflection;


namespace VisitPop.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var myEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appSettings = myEnv == null ? $"appsettings.json" : $"appsettings.{myEnv}.json";

            //  Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile(appSettings)
                .Build();

            // Initializar Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            try
            {
                Log.Information("Starting application");
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception e)
            {
                Log.Error(e, "The application failed to start correctly");
                throw;
            }
            finally
            {
                Log.Information("Shutthing down application");
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(typeof(Startup).GetTypeInfo().Assembly.FullName)
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseKestrel();
                });
    }
}
