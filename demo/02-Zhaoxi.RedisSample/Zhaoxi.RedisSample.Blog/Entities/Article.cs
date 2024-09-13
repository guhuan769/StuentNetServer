namespace Zhaoxi.RedisSample.Blog.Entities
{
    public class Article
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string ReleaseTime { get; set; }
    }
}
