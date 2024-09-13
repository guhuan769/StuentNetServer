using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Zhaoxi.RedisSample.Blog.RedisConstants;

namespace Zhaoxi.RedisSample.Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRedisDatabase _redis;

        public UserController(IRedisDatabase redisDatabase)
        {
            _redis = redisDatabase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSign(long id)
        {
            var mouth = DateTime.Now.ToString("yyyyMM");
            var key = $"{UserConstant.UserSignIn}{id}:{mouth}";
            await _redis.Database.StringSetBitAsync(key, DateTime.Now.Day - 1, true);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSign(long id, string mouth)
        {
            var key = $"{UserConstant.UserSignIn}{id}:{mouth}";
            var isSign = await _redis.Database.StringGetBitAsync(key, DateTime.Now.Day - 1);
            return Ok(isSign);
        }
    }
}
