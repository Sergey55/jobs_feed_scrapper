namespace JobsFeedScraper.Configuration
{
    public class FeedItem
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public override string ToString()
        {
            return $"{Name}\t{Url}";
        }
    }    
}