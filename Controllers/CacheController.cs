using Microsoft.AspNetCore.Mvc;
using RedisWebAPI.Services;

namespace RedisWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController : ControllerBase
{
    private readonly IRedisCacheService _cache;

    public CacheController(IRedisCacheService cache)
    {
        _cache = cache;
    }

    // GET /cache/{key}
    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key)
    {
        var value = await _cache.GetAsync(key);
        if (value is null) return NotFound();
        return Ok(new { Key = key, Value = value });
    }

    // POST /cache
    // Body: { "key": "...", "value": "...", "expirySeconds": 60 }
    [HttpPost]
    public async Task<IActionResult> Set([FromBody] CacheItem item)
    {
        await _cache.SetAsync(item.Key, item.Value, 
            item.ExpirySeconds.HasValue 
                ? TimeSpan.FromSeconds(item.ExpirySeconds.Value) 
                : null);
        return CreatedAtAction(nameof(Get), new { key = item.Key }, item);
    }

    public record CacheItem(string Key, string Value, int? ExpirySeconds);
}