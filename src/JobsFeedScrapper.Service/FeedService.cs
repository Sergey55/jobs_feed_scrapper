using JobsFeedScrapper.Configuration.Models;
using JobsFeedScrapper.EventHub;
using JobsFeedScrapper.FeedServiceClient;
using JobsFeedScrapper.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobsFeedScrapper.Service
{
    public class FeedService : BackgroundService
    {
        private FeedClient _feedClient;

        private readonly ILogger<FeedClient> _logger;

        private readonly IConfiguration _configuration;

        private readonly IJobsFeedEventHub _eventHub;

        public FeedService(
            ILogger<FeedClient> logger, 
            IConfiguration configuration,
            IJobsFeedEventHub eventHub)
        {
            _logger = logger;
            _configuration = configuration;
            _eventHub = eventHub;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Feed Service Client");

            _feedClient = new FeedClient(GenerateFeedClientConfig(), _logger, _eventHub);

            return _feedClient.StartAsync(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Feed Service Client");

            _feedClient?.StopAsync();
            _feedClient = null;

            return Task.CompletedTask;
        }

        private IFeedClientConfig GenerateFeedClientConfig()
        {
            return new FeedClientConfig()
            {
                PollIntervar = 5 * 60 * 1000,

                Feeds = _configuration.GetSection("feeds").Get<List<FeedItem>>()
            };
        }
    }
}
