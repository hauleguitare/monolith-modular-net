using Microsoft.Extensions.Caching.Distributed;
using MonolithModularNET.Extensions.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonolithModularNET.Extensions.Shared.Cache;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    public T? Get<T>(string key)
    {
        var rawData = distributedCache.GetString(key);

        return string.IsNullOrEmpty(rawData) ? default : JsonConvert.DeserializeObject<T>(rawData,new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        });
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var rawData = await distributedCache.GetStringAsync(key, cancellationToken);

        var result = string.IsNullOrEmpty(rawData) ? default : JsonConvert.DeserializeObject<T>(rawData, new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        });

        return result;
    }


    public bool Set(string key, object value, TimeSpan expirationTimeSpan)
    {
        try
        {
            var rawData = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            distributedCache.SetString(key, rawData, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SetAsync(string key, object value, TimeSpan expirationTimeSpan, CancellationToken cancellationToken = default)
    {
        try
        {
            var rawData = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await distributedCache.SetStringAsync(key, rawData, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan), cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Set(string key, string value, TimeSpan expirationTimeSpan)
    {
        try
        {
            distributedCache.SetString(key, value, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SetAsync(string key, string value, TimeSpan expirationTimeSpan, CancellationToken cancellationToken = default)
    {
        try
        {
            await distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions().SetSlidingExpiration(expirationTimeSpan), cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    public void Remove(string key)
    {
        distributedCache.Remove(key);
    }
}