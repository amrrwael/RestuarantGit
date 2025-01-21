using Delivery.Resutruant.API.Services.Interfaces;
using System.Collections.Concurrent;

namespace Delivery.Resutruant.API.Services
{
    public class BlacklistService : IBlacklistService
    {
        // Thread-safe dictionary to store tokens and their expiration times
        private readonly ConcurrentDictionary<string, DateTime> _blacklistedTokens = new();

        // Add a token to the blacklist with its expiration time
        public Task AddToBlacklistAsync(string token, DateTime expiration)
        {
            _blacklistedTokens[token] = expiration;
            return Task.CompletedTask;
        }

        // Check if a token is blacklisted
        public Task<bool> IsTokenBlacklistedAsync(string token)
        {
            if (_blacklistedTokens.TryGetValue(token, out var expiration))
            {
                // Remove the token if it has expired
                if (DateTime.UtcNow > expiration)
                {
                    _blacklistedTokens.TryRemove(token, out _);
                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }

}
