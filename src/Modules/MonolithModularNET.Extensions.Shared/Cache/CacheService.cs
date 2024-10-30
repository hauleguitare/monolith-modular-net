using Microsoft.Extensions.Caching.Distributed;
using MonolithModularNET.Extensions.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonolithModularNET.Extensions.Shared.Cache;

public abstract class CacheService<TDistributeCache>(TDistributeCache distributeCache) : ICacheService
    where TDistributeCache : IDistributedCache
{
    private readonly TDistributeCache _distributeCache = distributeCache;

    public T? Get<T>(string key)
    {
        var rawData = _distributeCache.GetString(key);

        return string.IsNullOrEmpty(rawData) ? default : JsonConvert.DeserializeObject<T>(rawData,new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        });
    }

    public string? Get(string key)
    {
        return _distributeCache.GetString(key);
    }

    public virtual async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var rawData = await _distributeCache.GetStringAsync(key, cancellationToken);

        var result = string.IsNullOrEmpty(rawData) ? default : JsonConvert.DeserializeObject<T>(rawData, new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        });

        return result;
    }

    public virtual async Task<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _distributeCache.GetStringAsync(key, cancellationToken);
    }
    


    public virtual bool Set(string key, object value, TimeSpan expirationTimeSpan)
    {
        try
        {
            var rawData = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            _distributeCache.SetString(key, rawData, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SetAsync(string key, byte[] bytes, TimeSpan expirationTimeSpan, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributeCache.SetAsync(key, bytes, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan), cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public virtual async Task<bool> SetAsync(string key, object value, TimeSpan expirationTimeSpan, CancellationToken cancellationToken = default)
    {
        try
        {
            var rawData = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await _distributeCache.SetStringAsync(key, rawData, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan), cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public virtual bool Set(string key, string value, TimeSpan expirationTimeSpan)
    {
        try
        {
            _distributeCache.SetString(key, value, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Set(string key, byte[] bytes, TimeSpan expirationTimeSpan)
    {
        try
        {
            _distributeCache.Set(key, bytes, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public virtual async Task<bool> SetAsync(string key, string value, TimeSpan expirationTimeSpan, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributeCache.SetStringAsync(key, value, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan), cancellationToken);
            return true;
        }
        catch (ArgumentNullException ex)
        {
            return false;
        }
    }


    public virtual void Remove(string key)
    {
        _distributeCache.Remove(key);
    }

    public virtual async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributeCache.RemoveAsync(key, cancellationToken);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}