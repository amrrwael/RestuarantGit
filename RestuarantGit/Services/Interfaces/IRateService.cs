namespace Delivery.Resutruant.API.Services.Interfaces
{
    public interface IRateService
    {
        Task<bool> CanRateDishAsync(string userId, Guid dishId);
        Task RateDishAsync(string userId, Guid dishId, int rate);
        Task<double> GetAverageRatingAsync(Guid dishId);
    }
}
