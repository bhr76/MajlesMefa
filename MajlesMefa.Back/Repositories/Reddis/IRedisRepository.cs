namespace MajlesMefa.Back.Repositories.Reddis
{
    public interface IRedisRepository
    {
        Task<bool> SetUserSessionAsync(string userId, string sessionToken, TimeSpan expiry);
        Task<string> GetUserSessionAsync(string userId);
        Task<bool> RemoveUserSessionAsync(string userId);
        Task<bool> IsUserLoggedInAsync(string userId);
        Task<bool> AddToBlacklistAsync(string token, TimeSpan expiry);
        Task<bool> IsTokenBlacklistedAsync(string token);

    }

}
