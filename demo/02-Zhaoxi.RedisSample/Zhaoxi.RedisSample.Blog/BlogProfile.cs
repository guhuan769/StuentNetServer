using AutoMapper;
using Zhaoxi.RedisSample.Blog.Dtos;
using Zhaoxi.RedisSample.Blog.Entities;

namespace Zhaoxi.RedisSample.Blog
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<CreateArticleDto, Article>();
            CreateMap<Article, ArticleDto>();
            CreateMap<Article, Dictionary<string, object>>()
                .ConstructUsing(article =>
                    new Dictionary<string, object>
                    {
                        { "Id", article.Id },
                        { "Title", article.Title },
                        { "Content", article.Content },
                        { "Author", article.Author },
                        { "ReleaseTime", article.ReleaseTime },
                    }
                );
        }
    }
}
