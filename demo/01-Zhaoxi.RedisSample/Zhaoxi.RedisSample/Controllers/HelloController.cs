using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Zhaoxi.RedisSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly IRedisDatabase _redis;

        public HelloController(IRedisDatabase redisDatabase)
        {
            _redis = redisDatabase;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _redis.AddAsync("Hello", DateTime.Now.ToString(CultureInfo.CurrentCulture));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _redis.GetAsync<string>("Hello");
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _redis.RemoveAsync("Hello");
            return Ok();
        }
    }
}
