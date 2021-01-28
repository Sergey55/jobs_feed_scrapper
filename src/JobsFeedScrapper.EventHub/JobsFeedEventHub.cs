using JobsFeedScrapper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobsFeedScrapper.EventHub
{
    public class JobsFeedEventHub : IJobsFeedEventHub
    {
        public event NewJobsEventHandler NewJobs;

        public JobsFeedEventHub()
        { }

        public void RaiseNewJobs(FeedItem feed, IEnumerable<JobDescription> jobs)
        {
            Task.Run(() => NewJobs?.Invoke(this, new NewJobsEventArgs(feed, jobs)));
        }
    }
}
