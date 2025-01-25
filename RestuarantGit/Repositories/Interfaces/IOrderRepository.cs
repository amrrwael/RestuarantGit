using Delivery.Resutruant.API.Models.Domain;

namespace Delivery.Resutruant.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetAllOrdersByUserEmailAsync(string userEmail);
        Task<Order> GetOrderByIdAsync(Guid orderId, string userEmail);

    }
}
