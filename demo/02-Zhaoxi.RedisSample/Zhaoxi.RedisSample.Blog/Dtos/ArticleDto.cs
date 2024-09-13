namespace Zhaoxi.RedisSample.Blog.Dtos
{
    public class ArticleDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string ReleaseTime { get; set; }
        public long PageView { get; set; }
    }
}
