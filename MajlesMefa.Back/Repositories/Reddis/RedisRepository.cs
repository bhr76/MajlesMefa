using System.Text.Json;
using MajlesMefa.Back.Utilities.Convertor;
using StackExchange.Redis;

namespace MajlesMefa.Back.Repositories.Reddis
{
    public class RedisRepository : IRedisRepository//<T> where T : new()
    {
        private readonly IConnectionMultiplexer _redis; // تغییر به Interface
        private readonly IDatabase _database;

        public RedisRepository(IConnectionMultiplexer redis) // تغییر پارامتر به Interface
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<bool> SetUserSessionAsync(string userId, string sessionToken, TimeSpan expiry)
        {
            var key = $"user_session:{userId}";
            return await _database.StringSetAsync(key, sessionToken, expiry);
        }

        public async Task<string> GetUserSessionAsync(string userId)
        {
            var key = $"user_session:{userId}";
            return await _database.StringGetAsync(key);
        }

        public async Task<bool> RemoveUserSessionAsync(string userId)
        {
            var key = $"user_session:{userId}";
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> IsUserLoggedInAsync(string userId)
        {
            var key = $"user_session:{userId}";
            return await _database.KeyExistsAsync(key);
        }

        public async Task<bool> AddToBlacklistAsync(string token, TimeSpan expiry)
        {
            var key = $"blacklist_token:{token}";
            return await _database.StringSetAsync(key, "blacklisted", expiry);
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var key = $"blacklist_token:{token}";
            return await _database.KeyExistsAsync(key);
        }
    }


}
