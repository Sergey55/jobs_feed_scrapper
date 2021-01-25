using JobsFeedScraper.Configuration;
using JobsFeedScrapper.FeedServiceClient;
using System.Collections.Generic;

namespace JobsFeedScrapper.Configuration.Models
{
    public class FeedClientConfig : IFeedClientConfig
    {
        public int PollIntervar { get; set; }
        public IList<FeedItem> Feeds { get; set; }
    }
}
