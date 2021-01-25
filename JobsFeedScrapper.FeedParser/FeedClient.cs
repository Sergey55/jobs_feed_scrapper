using JobsFeedScraper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Parsers.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JobsFeedScrapper.FeedServiceClient
{
    public class FeedClient
    {
        public delegate void NewJobsEventHandler(object sender, NewJobsEventArgs e);

        public event NewJobsEventHandler NewJobs;

        private readonly IFeedClientConfig _config;

        private readonly ILogger<FeedClient> _logger;

        private CancellationTokenSource _cts;

        public FeedClient(IFeedClientConfig config, ILogger<FeedClient> logger)
        {
            this._config = config;
            this._logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            CancelIfRunning();

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            var task = Task.Factory.StartNew(async () => {

                while (!cancellationToken.IsCancellationRequested)
                {
                    var tasks =
                        _config.Feeds.Select(feed => ProcessFeed(feed, _cts.Token))
                        .ToArray();

                    await Task.WhenAll(tasks);

                    await Task.Delay(_config.PollIntervar, _cts.Token);
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            task.ContinueWith(t => _logger.LogError(t.Exception, t.Exception.Message), continuationOptions: TaskContinuationOptions.OnlyOnFaulted);

            return task.Unwrap();
        }

        public void StopAsync()
        {
            CancelIfRunning();
        }

        private async Task ProcessFeed(FeedItem feed, CancellationToken token)
        {
            try
            {
                _logger.LogInformation($"Processing feed `{feed.Name}`.");

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(feed.Url, token);

                    var content = await response.Content.ReadAsStringAsync();

                    var parser = new RssParser();

                    var rss = parser.Parse(content);

                    RaiseNewJobs(feed, rss);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        private void RaiseNewJobs(FeedItem feed, IEnumerable<RssSchema> jobs)
        {
            NewJobs?.Invoke(this, new NewJobsEventArgs(feed, jobs.ToList()));
        }

        private void CancelIfRunning()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                _cts = null;
            }
        }
    }
}
