using JobsFeedScrapper.Models;
using System.Collections.Generic;

namespace JobsFeedScrapper.FeedServiceClient
{
    public interface IFeedClientConfig
    {
        int PollIntervar { get; set; }

        IList<FeedItem> Feeds { get; set; }
    }
}
