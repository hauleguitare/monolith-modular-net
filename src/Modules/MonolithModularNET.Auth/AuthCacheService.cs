using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Cache;

namespace MonolithModularNET.Auth;

public interface IAuthDistributedCache : IDistributedCache;

public class AuthDistributedCache(IOptions<RedisCacheOptions> optionsAccessor)
    : RedisCache(optionsAccessor), IAuthDistributedCache;


public interface IAuthCacheService : ICacheService;

public class AuthCacheService(IAuthDistributedCache distributeCache) : CacheService<IAuthDistributedCache>(distributeCache), IAuthCacheService
{
    
}