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

      

        public async Task CreateBasketAsync(Basket basket)
        {
            Console.WriteLine("Creating a new basket.");
            await _context.Baskets.AddAsync(basket);
            await _context.SaveChangesAsync();
        }

        
    }
}
