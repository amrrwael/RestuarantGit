using Delivery.Resutruant.API.DataBase;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.Enums;
using Delivery.Resutruant.API.Models.Pagination;
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

        public async Task<PagedResult<Dish>> GetAllAsync([FromQuery] Category? category = null,
    [FromQuery] bool? isVegetarian = null,
    [FromQuery] SortOption sortOption = SortOption.NameAsc,
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

            
            // Apply sorting
            switch (sortOption)
            {
                case SortOption.RatingAsc:
                    dishRatingsQuery = dishRatingsQuery.OrderBy(d => d.AverageRating ?? double.MinValue);
                    break;
                case SortOption.RatingDesc:
                    dishRatingsQuery = dishRatingsQuery.OrderByDescending(d => d.AverageRating ?? double.MinValue);
                    break;
                case SortOption.NameAsc:
                    dishRatingsQuery = dishRatingsQuery.OrderBy(d => d.Dish.Name);
                    break;
                case SortOption.NameDesc:
                    dishRatingsQuery = dishRatingsQuery.OrderByDescending(d => d.Dish.Name);
                    break;
                case SortOption.PriceAsc:
                    dishRatingsQuery = dishRatingsQuery.OrderBy(d => d.Dish.Price);
                    break;
                case SortOption.PriceDesc:
                    dishRatingsQuery = dishRatingsQuery.OrderByDescending(d => d.Dish.Price);
                    break;
            }

            // Fetch and paginate the results
            var dishesWithRatings = await dishRatingsQuery
                .Skip((currentPage - 1) * 5)
                .Take(5)
                .ToListAsync();

            var dishes = dishesWithRatings.Select(d => d.Dish).ToList();

            // Create pagination
            var pagination = new Pagination
            {
                Size = 5,
                Count = (int)Math.Ceiling((double)query.Count() / 5),
                Current = currentPage
            };

            return new PagedResult<Dish>
            {
                Dishes = dishes,
                Pagination = pagination
            };
        }



        public async Task<Dish> GetByIdAsync(Guid id)
        {
            return await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        }

        
    }
}



