using Pastebin_api.Data;
using StackExchange.Redis;

namespace Pastebin_api.Services
{
    public class RedisCleanupWorker : BackgroundService
    {
        private readonly IDatabase _scheduledTaskDb;
        private readonly IServiceProvider _serviceProvider;
        public RedisCleanupWorker(IConnectionMultiplexer redisMultiplexer, IServiceProvider serviceProvider)
        {
            _scheduledTaskDb = redisMultiplexer.GetDatabase(1);
            _serviceProvider = serviceProvider; // To access other services like PostgreSQL or S3
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessExpiredEntries();
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
        private async Task ProcessExpiredEntries()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var expiredTasks = _scheduledTaskDb.SortedSetRangeByScore("entry_expirations", stop: now);

            foreach (var taskId in expiredTasks)
            {
                // Remove the task from the Redis sorted set
                _scheduledTaskDb.SortedSetRemove("entry_expirations", taskId);

                // Process the task (delete from PostgreSQL and S3)
                await DeleteExpiredEntry(taskId.ToString());
            }
        }
        private async Task DeleteExpiredEntry(string taskId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Resolve the scoped services
                var dbContext = scope.ServiceProvider.GetRequiredService<PastebinDbContext>();
                var s3Service = scope.ServiceProvider.GetRequiredService<S3Service>();

                var textBlock = await dbContext.TextBlocks.FindAsync(taskId);

                if (textBlock != null)
                {
                    dbContext.TextBlocks.Remove(textBlock);
                    await dbContext.SaveChangesAsync(); 
                }
                await s3Service.DeleteTextAsync(taskId);
            }
        }
    }
}
