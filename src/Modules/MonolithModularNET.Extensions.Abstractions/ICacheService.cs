namespace MonolithModularNET.Extensions.Abstractions;

public interface ICacheService
{
    T? Get<T>(string key);
    string? Get(string key);
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task<string?> GetAsync(string key, CancellationToken cancellationToken = default);
    bool Set(string key, object value, TimeSpan expirationTimeSpan);
    bool Set(string key, string value, TimeSpan expirationTimeSpan);
    bool Set(string key, byte[] bytes, TimeSpan expirationTimeSpan);
    Task<bool> SetAsync(string key, byte[] bytes, TimeSpan expirationTimeSpan, CancellationToken cancellationToken = default);
    Task<bool> SetAsync(string key, object value, TimeSpan expirationTimeSpan, CancellationToken cancellationToken = default);
    Task<bool> SetAsync(string key, string value, TimeSpan expirationTimeSpan, CancellationToken cancellationToken = default);
    void Remove(string key);
    Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);
}