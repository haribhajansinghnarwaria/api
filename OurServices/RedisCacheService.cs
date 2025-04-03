using api.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace api.OurServices
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
             var cachedData = await _cache.GetStringAsync(key);
            if(cachedData is null)
            {
                return default(T?);
            }
            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task<bool> SetDataAsync<T>(string key, T data)
        {
            var serializationData = JsonSerializer.Serialize(data);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };
            await _cache.SetStringAsync(key, serializationData,options);
            return true;
        }
    }
}
