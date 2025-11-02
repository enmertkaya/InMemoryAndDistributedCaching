using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("set")]
        public async Task<IActionResult> Set(string name, string surname)
        {
            // Clear any earlier cache entries that might have a conflicting type.
            await _distributedCache.RemoveAsync("name");
            await _distributedCache.RemoveAsync("surname");

            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));

            await _distributedCache.SetStringAsync("name", name, cacheOptions);
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), cacheOptions);
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("name");
            var surnameBinary = await _distributedCache.GetAsync("surname");
            var surname = surnameBinary is null ? null : Encoding.UTF8.GetString(surnameBinary);

            return Ok(new
            {
                name,
                surname
            });
        }
    }
}
