using System;

namespace JobsFeedScrapper.Models
{
    public class FeedItem
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime LastCheckDate { get; set; }

        public override string ToString()
        {
            return $"{Name}\t{Url}";
        }
    }    
}