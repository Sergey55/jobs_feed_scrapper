using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobsFeedScrapper.Service
{
    public class TestBot : BackgroundService
    {
        public TestBot(ILogger<TestBot> logger)
        {
            Console.WriteLine("Ctor");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Start");

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stop");

            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("ExecuteAsync");

            return Task.CompletedTask;
        }
    }
}
