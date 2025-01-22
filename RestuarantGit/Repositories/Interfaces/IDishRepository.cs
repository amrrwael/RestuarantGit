using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.Enums;
using Delivery.Resutruant.API.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Resutruant.API.Repositories.Interfaces
{
    public interface IDishRepository
    {
        Task<PagedResult<Dish>> GetAllAsync([FromQuery] Category? category = null,
            [FromQuery] bool? isVegetarian = null,
            [FromQuery] SortOption sortOption = SortOption.NameAsc,
            int currentPage = 1);
        Task<Dish> GetByIdAsync(Guid dishId);

    }
}
