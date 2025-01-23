using System;
using System.Threading.Tasks;
using Delivery.Resutruant.API.Models.DTO;

namespace Delivery.Resutruant.API.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketDto> GetUserBasketAsync(string userEmail);
        Task<bool> AddDishToBasketAsync(string userEmail, Guid dishId);
    }
}
