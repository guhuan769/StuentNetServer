using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Zhaoxi.RedisSample.Blog.Dtos;
using Zhaoxi.RedisSample.Blog.Entities;
using Zhaoxi.RedisSample.Blog.RedisConstants;

namespace Zhaoxi.RedisSample.Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Article01Controller : ControllerBase
    {
        private readonly IRedisDatabase _redis;
        private readonly IMapper _mapper;

        public Article01Controller(IRedisDatabase redisDatabase, IMapper mapper)
        {
            _redis = redisDatabase;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var article = await _redis.GetAsync<Article>(ArticleConstant.ArticleData + id);
            var articleDto = _mapper.Map<ArticleDto>(article);
            articleDto.PageView = await _redis.Database.StringIncrementAsync(ArticleConstant.ArticlePageView + id);
            return Ok(articleDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleDto createArticle)
        {
            var article = _mapper.Map<Article>(createArticle);
            article.Id = await _redis.Database.StringIncrementAsync(ArticleConstant.ArticleCount);
            article.ReleaseTime = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            await _redis.AddAsync(ArticleConstant.ArticleData + article.Id, article);
            return CreatedAtAction("Get", article);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _redis.RemoveAsync(ArticleConstant.ArticleData + id);
            await _redis.RemoveAsync(ArticleConstant.ArticlePageView + id);
            await _redis.Database.StringDecrementAsync(ArticleConstant.ArticleCount);
            return Ok();
        }
    }
}
