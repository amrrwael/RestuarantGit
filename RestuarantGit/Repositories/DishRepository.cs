using Delivery.Resutruant.API.DataBase;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.Enums;
using Delivery.Resutruant.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Resutruant.API.Repositories
{
    // DishRepository provides methods for fetching and filtering dishes with support for pagination and sorting
    public class DishRepository : IDishRepository
    {
        private readonly ApplicationDbContext _dbContext;

        // Constructor to inject the database context
        public DishRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<<Dish>> GetAllAsync([FromQuery] Category? category = null,
    [FromQuery] bool? isVegetarian = null,
    [FromQuery] int currentPage = 1)
        {
            var query = _dbContext.Dishes.AsQueryable();

            // Apply filters for vegetarian and category
            if (isVegetarian.HasValue && isVegetarian.Value)
            {
                query = query.Where(d => d.IsVegeterian == true);
            }
            if (category.HasValue)
            {
                query = query.Where(d => d.Category == category.Value);
            }

 
            // Fetch and paginate the results
            var dishesWithRatings = await dishRatingsQuery
                .Skip((currentPage - 1) * 5)
                .Take(5)
                .ToListAsync();

            var dishes = dishesWithRatings.Select(d => d.Dish).ToList();

        }



        public async Task<Dish> GetByIdAsync(Guid id)
        {
            return await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        }

        
    }
}



