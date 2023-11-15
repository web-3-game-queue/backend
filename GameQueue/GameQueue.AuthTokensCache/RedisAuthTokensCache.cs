using Microsoft.Extensions.Caching.Distributed;

namespace GameQueue.AuthTokensCache;

public class RedisAuthTokensCache
{
    private IDistributedCache cache;

    public RedisAuthTokensCache(IDistributedCache cache)
    {
        this.cache = cache;
    }

    public async Task<string?> GetKey(string key, CancellationToken token = default)
    {
        return await cache.GetStringAsync(key, token);
    }

    public async Task SetKeyValue(string key, string value, CancellationToken token = default)
    {
        await cache.SetStringAsync(key, value, token);
    }

    public async Task RemoveKey(string key, CancellationToken token = default)
    {
        await cache.RemoveAsync(key, token);
    }
}
