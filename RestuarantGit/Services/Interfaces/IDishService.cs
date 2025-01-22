using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Models.Enums;
using Delivery.Resutruant.API.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Resutruant.API.Services.Interfaces
{
    public interface IDishService
    {
        Task<PagedResult<DishDto>> GetAllDishesAsync([FromQuery] Category? category = null,
            [FromQuery] bool? isVegetarian = null,
            [FromQuery] SortOption sortOption = SortOption.NameAsc,
            int currentPage = 1);
        Task<DishDto> GetDishByIdAsync(Guid id);

    }
}
