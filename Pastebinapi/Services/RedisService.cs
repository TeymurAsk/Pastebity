using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Pastebin_api.Services
{
    public class RedisService
    {
        private readonly IDistributedCache? _cache;
        private readonly IConnectionMultiplexer _redisMultiplexer;
        private readonly IDatabase _cacheDb;
        private readonly IDatabase _scheduledTaskDb;
        public RedisService(IDistributedCache cache, IConnectionMultiplexer redisMultiplexer)
        {
            _redisMultiplexer = redisMultiplexer;
            _scheduledTaskDb = _redisMultiplexer.GetDatabase(1);
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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(4),
                SlidingExpiration = TimeSpan.FromMinutes(1),
            };
            _cache?.SetString(key, JsonSerializer.Serialize(data), options);
        }
        public void ScheduleTask(string taskId, DateTime expirationTime)
        {
            var expirationTimestamp = new DateTimeOffset(expirationTime).ToUnixTimeSeconds();
            _scheduledTaskDb.SortedSetAdd("entry_expirations", taskId, expirationTimestamp);
        }
    }
}
