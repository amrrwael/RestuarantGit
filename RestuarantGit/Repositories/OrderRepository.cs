using Delivery.Resutruant.API.Repositories.Interfaces;
using Delivery.Resutruant.API.DataBase;
using Delivery.Resutruant.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Resutruant.API.Repositories
{
    // Repository for managing orders, including creation, retrieval, and updates.
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject the database context
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Creates a new order and saves it to the database.
        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Add the order to the database context
            await _context.Orders.AddAsync(order);
            // Save the changes to the database
            await _context.SaveChangesAsync();
            return order;
        }

    }
}
