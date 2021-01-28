using JobsFeedScrapper.Models;
using System.Collections.Generic;

namespace JobsFeedScrapper.EventHub
{
    public class NewJobsEventArgs
    {
        public FeedItem Feed { get; set; }

        public IEnumerable<JobDescription> Jobs { get; set; }

        public NewJobsEventArgs(FeedItem feed, IEnumerable<JobDescription> jobs)
        {
            Feed = feed;
            Jobs = jobs;
        }
    }
}