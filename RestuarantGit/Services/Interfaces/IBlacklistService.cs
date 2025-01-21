namespace Delivery.Resutruant.API.Services.Interfaces
{
    public interface IBlacklistService
    {
        Task AddToBlacklistAsync(string token, DateTime expiration);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
