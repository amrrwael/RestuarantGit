using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Resutruant.API.Repositories.Interfaces
{
    public interface IDishRepository
    {
        Task<<Dish>> GetAllAsync([FromQuery] Category? category = null,
            [FromQuery] bool? isVegetarian = null,
            int currentPage = 1);
        Task<Dish> GetByIdAsync(Guid dishId);

    }
}
