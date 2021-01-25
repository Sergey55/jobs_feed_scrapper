using JobsFeedScraper.Configuration;
using JobsFeedScrapper.Configuration.Models;
using JobsFeedScrapper.FeedServiceClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobsFeedScrapper.Service
{
    public class FeedService : BackgroundService
    {
        private FeedClient _feedClient;

        private readonly ILogger<FeedClient> _logger;

        private readonly IConfiguration _configuration;

        public FeedService(ILogger<FeedClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Feed Service Client");

            _feedClient = new FeedClient(GenerateFeedClientConfig(), _logger);

            _feedClient.NewJobs += _feedClient_NewJobs;

            return _feedClient.StartAsync(stoppingToken);
        }

        private void _feedClient_NewJobs(object sender, NewJobsEventArgs e)
        {
            _logger.LogInformation($"{e.Feed.Name}/{e.Jobs.Count()}");
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
                PollIntervar = 60 * 1000,

                Feeds = _configuration.GetSection("feeds").Get<List<FeedItem>>()
            };
        }
    }
}
