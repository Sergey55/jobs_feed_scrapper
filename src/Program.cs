using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobsFeedScraper
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
                    services.AddHostedService<Scraper>();
                });
    }
}
