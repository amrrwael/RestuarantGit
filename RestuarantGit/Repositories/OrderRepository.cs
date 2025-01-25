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

        // Retrieves all orders associated with a specific user by their email.
        public async Task<IEnumerable<Order>> GetAllOrdersByUserEmailAsync(string userEmail)
        {
            return await _context.Orders
                .Where(o => o.UserEmail == userEmail)
                .ToListAsync();
        }

        // Retrieves a specific order by its ID and user email, including its items.
        public async Task<Order> GetOrderByIdAsync(Guid orderId, string userEmail)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserEmail == userEmail);
        }

        // Updates an existing order in the database.
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        // Retrieves orders by a specific user and order status, including the items in each order.
        public async Task<List<Order>> GetOrdersByUserWithStatusAsync(string userEmail, string status)
        {
            // Query the database for orders with the specified user email and status
            return await _context.Orders
                .Where(order => order.UserEmail == userEmail && order.Status == status)
                .Include(order => order.Items)
                .ThenInclude(orderItem => orderItem.Dish)
                .ToListAsync();
        }
    }
}
