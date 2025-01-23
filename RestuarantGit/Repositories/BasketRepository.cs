using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Delivery.Resutruant.API.DataBase;

namespace Delivery.Resutruant.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ApplicationDbContext _context;

        public BasketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dish> GetByIdAsync(Guid dishId)
        {
            Console.WriteLine($"Fetching dish with ID: {dishId}");
            return await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        }

        public async Task<Basket> GetBasketByUserEmailAsync(string userEmail)
        {
            Console.WriteLine($"Fetching basket for user: {userEmail}");
            var basket = await _context.Baskets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => EF.Functions.Like(b.UserEmail, userEmail));

            if (basket != null && basket.Items == null)
            {
                Console.WriteLine("Basket found, initializing items list.");
                basket.Items = new List<BasketItem>();
            }
            else if (basket == null)
            {
                Console.WriteLine("Basket not found.");
            }

            return basket;
        }

        public async Task CreateBasketAsync(Basket basket)
        {
            Console.WriteLine("Creating a new basket.");
            await _context.Baskets.AddAsync(basket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBasketAsync(Basket basket)
        {
            try
            {
                if (basket == null) throw new ArgumentNullException(nameof(basket), "Basket cannot be null.");
                if (basket.Items.Any(i => i == null)) throw new ArgumentNullException(nameof(basket.Items), "Basket items cannot be null.");

                Console.WriteLine($"Updating basket with ID: {basket.Id}");
                _context.Baskets.Update(basket);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating basket: {ex.Message}");
                throw;
            }
        }

        public async Task AddBasketItemAsync(string userEmail, BasketItem basketItem)
        {
            var basket = await GetBasketByUserEmailAsync(userEmail);
            if (basket == null)
            {
                Console.WriteLine($"Basket not found for user: {userEmail}");
                return;
            }

            if (basket.Items == null) basket.Items = new List<BasketItem>();

            basket.Items.Add(basketItem);
            Console.WriteLine($"Added item {basketItem.DishId} to basket for user: {userEmail}");
            await UpdateBasketAsync(basket);
        }
    
    }
}
