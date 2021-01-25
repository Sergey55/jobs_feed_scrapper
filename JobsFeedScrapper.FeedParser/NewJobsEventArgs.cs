using JobsFeedScraper.Configuration;
using Microsoft.Toolkit.Parsers.Rss;
using System.Collections.Generic;

namespace JobsFeedScrapper.FeedServiceClient
{
    public class NewJobsEventArgs
    {
        public FeedItem Feed { get; set; }

        public IEnumerable<RssSchema> Jobs { get; set; }

        public NewJobsEventArgs(FeedItem feed, IEnumerable<RssSchema> jobs)
        {
            Feed = feed;
            Jobs = jobs;
        }
    }
}