using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Parsers.Rss;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using JobsFeedScraper.Configuration;

namespace JobsFeedScraper
{
    public class Scrapper : BackgroundService
    {
        private const int POLL_INTERVAL = 60 * 1000;
        
        private IConfiguration _config;

        private IList<FeedItem> _feeds;

        private readonly ILogger<Scrapper> _logger;

        public Scrapper(ILogger<Scrapper> logger, IConfiguration config)
        {
            this._logger = logger;
            this._config = config;

            this._feeds = _config.GetSection("feeds").Get<List<FeedItem>>();                
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var parser = new RssParser();

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var feed in _feeds)
                {
                    using(var client = new HttpClient())
                    {
                        var response = await client.GetAsync(feed.Url, stoppingToken);
                        var content = await response.Content.ReadAsStringAsync();
                        var rss = parser.Parse(content);
                        System.Console.WriteLine($"{feed.Name}\t{rss.Count()}");
                    }

                    await Task.Delay(POLL_INTERVAL, stoppingToken);
                }
            }
        }
    }
}