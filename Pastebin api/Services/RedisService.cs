using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Pastebin_api.Services
{
    public class RedisService
    {
        private readonly IDistributedCache? _cache;
        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T? GetData<T>(string key)
        {
            var data = _cache?.GetString(key);
            if (data == null)
            {
                return default(T?);
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T data)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(6),
                SlidingExpiration = TimeSpan.FromMinutes(3),
            };
            _cache?.SetString(key, JsonSerializer.Serialize(data), options);
        }
    }
}
