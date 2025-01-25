using Delivery.Resutruant.API.Repositories.Interfaces;
using Delivery.Resutruant.API.Services.Interfaces;

namespace Delivery.Resutruant.API.Services
{
    
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;
        private readonly IOrderRepository _orderRepository;

        // Constructor to initialize dependencies via dependency injection
        public RateService(IRateRepository rateRepository, IOrderRepository orderRepository)
        {
            _rateRepository = rateRepository;
            _orderRepository = orderRepository;
        }

        // Checks whether a user is eligible to rate a dish.
        // A user can only rate a dish if they have previously ordered and received it.
        public async Task<bool> CanRateDishAsync(string useremail, Guid dishId)
        {
            var deliveredOrders = await _orderRepository.GetOrdersByUserWithStatusAsync(useremail, "Delivered");

            return deliveredOrders.Any(order =>
                order.Items.Any(item => item.DishId == dishId)
            );
        }

        // Allows a user to submit a rating for a dish. 
        // Validates the input parameters before saving the rating.
        public async Task RateDishAsync(string useremail, Guid dishId, int rate)
        {
            // Validate that the user email is not null or empty
            if (string.IsNullOrWhiteSpace(useremail))
                throw new ArgumentException("User email cannot be null or empty.", nameof(useremail));

            // Validate that the dish ID is not an empty GUID
            if (dishId == Guid.Empty)
                throw new ArgumentException("Dish ID cannot be an empty GUID.", nameof(dishId));

            // Validate that the rating is within the accepted range (1 to 10)
            if (rate < 1 || rate > 10)
                throw new ArgumentException("Rate must be between 1 and 10.", nameof(rate));

            await _rateRepository.AddOrUpdateRating(useremail, dishId, rate);
        }

        // Retrieves the average rating of a specific dish based on user ratings.
        public async Task<double> GetAverageRatingAsync(Guid dishId)
        {
            var ratings = await _rateRepository.GetRatingsByDishIdAsync(dishId);

            return ratings.Any() ? ratings.Average(r => r.Value) : 0.0;
        }
    }
}
