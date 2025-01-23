using Delivery.Resutruant.API.Models.Domain;

namespace Delivery.Resutruant.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task CreateBasketAsync(Basket basket);
    }
}
