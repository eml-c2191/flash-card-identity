using Identity.API.Services.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.API.Services
{
    public class IdentityCacheService : IIdentityCacheService
    {
        private readonly IMemoryCache _memoryCache;
        public IdentityCacheService
        (
            IMemoryCache memoryCache
        )
        {
            _memoryCache = memoryCache;
        }

        public void ClearAll()
        {
            _memoryCache.Remove(CacheEntries.OTPCodeEntry);
        }

        public async Task<TOutput?> GetAsync<TOutput>(string entry, Func<Task<TOutput>> task, int cacheTimeInSeconds)
        {
            return await _memoryCache.GetOrCreateAsync
            (
                entry,
                async cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheTimeInSeconds));
                    return await task();
                }
            );
        }
        public T? Get<T>(string entry)
        {
            return _memoryCache.Get<T>(entry);
        }
        public void Set<T>(string entry, T Data, int cacheTimeInSeconds)
        {
            _memoryCache.Set<T>(entry, Data, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheTimeInSeconds)
            });
        }

        public void Remove(string entry)
        {
            _memoryCache.Remove(entry);
        }
    }
}
