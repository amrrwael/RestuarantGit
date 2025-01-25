using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.Resutruant.API.Models.DTO;

namespace Delivery.Resutruant.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userEmail, OrderCreateDto orderCreateDto);
        Task<IEnumerable<OrderSummaryDto>> GetAllOrdersAsync(string userEmail);
        Task<OrderDto> GetOrderDetailsAsync(Guid orderId, string userEmail);
    }
}
