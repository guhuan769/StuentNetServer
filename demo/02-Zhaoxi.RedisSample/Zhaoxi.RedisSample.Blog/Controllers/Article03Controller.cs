using System.Collections;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Zhaoxi.RedisSample.Blog.Dtos;
using Zhaoxi.RedisSample.Blog.Entities;
using Zhaoxi.RedisSample.Blog.RedisConstants;
using Zhaoxi.RedisSample.Blog.RequestFeatures;

namespace Zhaoxi.RedisSample.Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Article03Controller : ControllerBase
    {
        private readonly IRedisDatabase _redis;
        private readonly IMapper _mapper;

        public Article03Controller(IRedisDatabase redisDatabase, IMapper mapper)
        {
            _redis = redisDatabase;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var article = await _redis.HashGetAllAsync<object>(ArticleConstant.ArticleData + id);
            var pageView = await _redis.Database.StringIncrementAsync(ArticleConstant.ArticlePageView + id);
            article.Add("PageView",pageView);
            
            return Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleDto createArticle)
        {
            var article = _mapper.Map<Article>(createArticle);
            article.Id = await _redis.Database.StringIncrementAsync(ArticleConstant.ArticleCount);
            article.ReleaseTime = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            var hashKey = ArticleConstant.ArticleData + article.Id;
            var articleDict = _mapper.Map<Dictionary<string, object>>(article);
            await _redis.HashSetAsync(hashKey, articleDict);

            await _redis.ListAddToLeftAsync(ArticleConstant.ArticleList, article.Id);

            return CreatedAtAction("Get", new { id = article.Id }, article);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _redis.RemoveAsync(ArticleConstant.ArticleData + id);
            await _redis.RemoveAsync(ArticleConstant.ArticlePageView + id);
            await _redis.Database.ListRemoveAsync(ArticleConstant.ArticleList, id);
            await _redis.Database.StringDecrementAsync(ArticleConstant.ArticleCount);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery]ArticleParameter parameter)
        {
            var start = (parameter.PageNumber - 1) * parameter.PageSize + 1;
            var end = parameter.PageNumber * parameter.PageSize;
            var ids =await _redis.Database.ListRangeAsync(ArticleConstant.ArticleList, start, end);

            var result = new List<Dictionary<string, object>>();
            foreach (var id in ids)
            {
                var article = new Dictionary<string, object>();
                var title = await _redis.HashGetAsync<object>(ArticleConstant.ArticleData + id, "Title");
                article.Add("id", id.ToString());
                article.Add("title", title);
                result.Add(article);
            }
            
            return Ok(result);
        }
    }
}
