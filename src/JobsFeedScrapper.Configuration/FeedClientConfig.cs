using JobsFeedScrapper.FeedServiceClient;
using JobsFeedScrapper.Models;
using System.Collections.Generic;

namespace JobsFeedScrapper.Configuration.Models
{
    public class FeedClientConfig : IFeedClientConfig
    {
        public int PollIntervar { get; set; }
        public IList<FeedItem> Feeds { get; set; }
    }
}
