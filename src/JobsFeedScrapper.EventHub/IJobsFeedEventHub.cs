using JobsFeedScrapper.Models;
using System.Collections.Generic;

namespace JobsFeedScrapper.EventHub
{
    public interface IJobsFeedEventHub
    {
        event NewJobsEventHandler NewJobs;

        void RaiseNewJobs(FeedItem feed, IEnumerable<JobDescription> jobs);
    }
}
