using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EFSecondLevelCache.Extensions;

public static class QueryCacheExtensions
{
    private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private const int AbsoluteExpirationSeconds = 10;

    private static string GetCacheKey(IQueryable query)
    {
        var queryString = query.ToQueryString();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(queryString));
        return Convert.ToBase64String(hash);
    }
    
    public static List<T> FromCache<T>(this IQueryable<T> query)
    {
        var key = GetCacheKey(query);
        
        var result =  _cache.GetOrCreate(key, cache =>
        {
            cache.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(AbsoluteExpirationSeconds);
            return query.ToList();
        }) ?? new List<T>();
        
        return result;
    }
    
    public static async Task<List<T>> FromCacheAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        var key = GetCacheKey(query);
        
        var result =  await _cache.GetOrCreateAsync(key, cache =>
        {
            cache.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(AbsoluteExpirationSeconds);
            return query.ToListAsync(cancellationToken);
        }) ?? new List<T>();
        
        return result;
    }

    public static void Clear()
    {
        _cache.Dispose();
        _cache = new MemoryCache(new MemoryCacheOptions());
    }
}