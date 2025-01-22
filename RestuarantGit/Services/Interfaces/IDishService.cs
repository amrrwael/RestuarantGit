using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Resutruant.API.Services.Interfaces
{
    public interface IDishService
    {
        Task<<DishDto>> GetAllDishesAsync([FromQuery] Category? category = null,
            [FromQuery] bool? isVegetarian = null
            );
        Task<DishDto> GetDishByIdAsync(Guid id);

    }
}
