using JobsFeedScrapper.EventHub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace JobsFeedScrapper.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((configurationBulder) => {
                    configurationBulder.AddJsonFile("appsettings.json", false, true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IJobsFeedEventHub, JobsFeedEventHub>();

                    services.AddHostedService<FeedService>();
                    services.AddHostedService<TelegramBotService>();
                });
    }
}
