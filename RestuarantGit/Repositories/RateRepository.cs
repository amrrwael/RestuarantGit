using Delivery.Resutruant.API.DataBase;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Resutruant.API.Repositories
{
    // Repository for managing ratings of dishes, including retrieval and updating operations.
    public class RateRepository : IRateRepository
    {
        private readonly ApplicationDbContext _dbContext;

        // Constructor to inject the database context
        public RateRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Retrieves all ratings associated with a specific dish.
        public async Task<List<Rating>> GetRatingsByDishIdAsync(Guid dishId)
        {
            // Query the database for all ratings linked to the given dish ID.
            return await _dbContext.Ratings
                .Where(r => r.DishId == dishId)
                .ToListAsync();
        }

        // Adds a new rating or updates an existing rating for a specific dish and user.
        public async Task AddOrUpdateRating(string useremail, Guid dishId, int value)
        {
            // Check if a rating already exists for the user and dish combination.
            var existingRating = await _dbContext.Ratings
                .FirstOrDefaultAsync(r => r.UserEmail == useremail && r.DishId == dishId);

            if (existingRating != null)
            {
                // Update the value of the existing rating if it exists.
                existingRating.Value = value;
                _dbContext.Ratings.Update(existingRating);
            }
            else
            {
                // Create a new rating if no existing rating is found.
                var newRating = new Rating
                {
                    UserEmail = useremail,
                    DishId = dishId,
                    Value = value
                };
                await _dbContext.Ratings.AddAsync(newRating);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
