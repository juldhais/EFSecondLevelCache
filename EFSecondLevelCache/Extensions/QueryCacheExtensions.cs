using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EFSecondLevelCache.Extensions;

public static class QueryCacheExtensions
{
    private static readonly IMemoryCache Cache = new MemoryCache(new MemoryCacheOptions());
    private static readonly TimeSpan AbsoluteExpirations = TimeSpan.FromSeconds(10000);
    
    public static List<T> FromCache<T>(this IQueryable<T> query)
    {
        var key = GetCacheKey(query);
        var result = Cache.Get<List<T>>(key);
        if (result != null) return result;
        
        result = query.ToList();
        Cache.Set(key, result, AbsoluteExpirations);
        return result;
    }
    
    public static async Task<List<T>> FromCacheAsync<T>(this IQueryable<T> query)
    {
        var key = GetCacheKey(query);
        var result = Cache.Get<List<T>>(key);
        if (result != null) return result;
        
        result = await query.ToListAsync();
        Cache.Set(key, result, AbsoluteExpirations);
        return result;
    }

    private static byte[] GetCacheKey(IQueryable query)
    {
        var queryString = query.ToQueryString();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(queryString));
        return hash;
    }
}