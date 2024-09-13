using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Zhaoxi.RedisSample.Blog.RedisConstants;

namespace Zhaoxi.RedisSample.Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IRedisDatabase _redis;

        public TagController(IRedisDatabase redisDatabase)
        {
            _redis = redisDatabase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTags(long id, string tags)
        {
            await _redis.SetAddAllAsync(ArticleConstant.ArticleTags + id, CommandFlags.None, tags.Split(","));

            // await _redis.SetAddAsync(ArticleConstant.ArticleTags + id, "1");
            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTags(long id)
        {
            var result = await _redis.SetMembersAsync<string>(ArticleConstant.ArticleTags + id);
            return Ok(result);
        }

        [HttpDelete] 
        public async Task<IActionResult> DeleteTagsById(long id, string tag)
        {
            await _redis.SetRemoveAsync(ArticleConstant.ArticleTags + id, tag);
            return Ok();
        }
    }
}
