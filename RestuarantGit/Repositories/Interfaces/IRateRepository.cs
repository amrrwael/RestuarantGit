using Delivery.Resutruant.API.Models.Domain;

namespace Delivery.Resutruant.API.Repositories.Interfaces
{
    public interface IRateRepository
    {
        Task<List<Rating>> GetRatingsByDishIdAsync(Guid dishId);
        Task AddOrUpdateRating(string useremail, Guid dishId, int value);
    }
}
