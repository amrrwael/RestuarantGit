using Delivery.Resutruant.API.Models.Domain;

namespace Delivery.Resutruant.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket> GetBasketByUserEmailAsync(string userEmail);
        Task CreateBasketAsync(Basket basket);
        Task UpdateBasketAsync(Basket basket);
        Task AddBasketItemAsync(string userEmail, BasketItem basketItem);
    }
}
