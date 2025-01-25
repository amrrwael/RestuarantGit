using AutoMapper;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Repositories.Interfaces;
using Delivery.Resutruant.API.Services.Interfaces;
using Delivery.Resutruant.API.Models.Domain;

namespace Delivery.Resutruant.API.Services
{
    // Service for handling order-related operations such as creating orders, retrieving orders, and confirming delivery.
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        // Constructor to initialize dependencies via dependency injection
        public OrderService(IOrderRepository orderRepository, IBasketRepository basketRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
            _mapper = mapper;
        }


        // Creates a new order for the specified user based on their current basket.
        public async Task<OrderDto> CreateOrderAsync(string userEmail, OrderCreateDto orderCreateDto)
        {
            var basket = await _basketRepository.GetBasketByUserEmailAsync(userEmail);
            if (basket == null || !basket.Items.Any())
            {

                return null;
            }

            // Create a new order using the basket's items.
            var order = new Order
            {
                UserEmail = userEmail,
                OrderTime = DateTime.UtcNow, // Current time as the order time.
                DeliveryTime = orderCreateDto.DeliveryTime, // Delivery time provided by the user.
                Status = "InProcess",
                Address = orderCreateDto.Address,
                Items = basket.Items.Select(item => new OrderItem
                {
                    DishId = item.DishId,
                    Name = item.Name,
                    Price = item.Price,
                    Amount = item.Amount,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };

            // Calculate the total price of the order.
            order.TotalPrice = order.Items.Sum(i => i.TotalPrice);

            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Clear the user's basket after the order is created.
            var userCart = await _basketRepository.GetBasketByUserEmailAsync(userEmail);
            if (userCart != null)
            {
                await _basketRepository.DeleteAllItemsByBasketIdAsync(userCart.Id);
            }

            return _mapper.Map<OrderDto>(createdOrder);
        }

        // Retrieves all orders for the specified user.
        public async Task<IEnumerable<OrderSummaryDto>> GetAllOrdersAsync(string userEmail)
        {
            var orders = await _orderRepository.GetAllOrdersByUserEmailAsync(userEmail);
            return _mapper.Map<IEnumerable<OrderSummaryDto>>(orders);
        }

        // Retrieves detailed information about a specific order for the user.
        public async Task<OrderDto> GetOrderDetailsAsync(Guid orderId, string userEmail)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, userEmail);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        /// Confirms an order by updating its status to "Delivered".
       
    }
}
