using System;

namespace JobsFeedScrapper.Models
{
    public class JobDescription
    {
        public DateTime PublishDate { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string FeedUrl { get; set; }
    }
}
