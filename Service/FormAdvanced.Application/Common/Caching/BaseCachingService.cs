using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace FormAdvanced.Application.Common.Caching
{
    public abstract class BaseCachingService
    {
        protected readonly IMemoryCache _cache;
        protected readonly TimeSpan _cacheDuration;

        protected BaseCachingService(IMemoryCache cache, TimeSpan cacheDuration)
        {
            _cache = cache;
            _cacheDuration = cacheDuration;
        }

        protected async Task<CachedResponse<T>> GetOrSetAsync<T>(string key, Func<Task<T>> factory)
        {
            if (_cache.TryGetValue(key, out T cachedValue))
            {
                return new CachedResponse<T>(cachedValue, true, DateTime.UtcNow);
            }

            var value = await factory();
            _cache.Set(key, value, _cacheDuration);
            return new CachedResponse<T>(value, false);
        }

        protected void InvalidateCache(string key)
        {
            _cache.Remove(key);
        }

        protected void InvalidateCacheByPrefix(string prefix)
        {
            // Note: This is a simple implementation. In a production environment,
            // you might want to use a more sophisticated cache invalidation strategy
            // or use a distributed cache that supports pattern-based invalidation
            /*
            var keys = _cache.GetKeys();
            foreach (var key in keys)
            {
                if (key.StartsWith(prefix))
                {
                    _cache.Remove(key);
                }
            }*/
        }
    }
} 