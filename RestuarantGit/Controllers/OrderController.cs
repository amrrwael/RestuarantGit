using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Delivery.Resutruant.API.Models;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Delivery.Resutruant.API.Controllers
{

    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private IActionResult HandleException(Exception ex)
        {
            return ex switch
            {
                UnauthorizedAccessException => Unauthorized(new { message = ex.Message }),
                KeyNotFoundException => NotFound(new { message = ex.Message }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." })
            };
        }

        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(List<OrderDto>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        [SwaggerOperation(Summary = "Get information about concrete order")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                // Retrieve the user's email from the JWT token
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Error = "User email not found or token is invalid." });
                }

                // Optional: Verify the user's role for access control (e.g., User or Admin)
                var isAuthorized = User.IsInRole("User") || User.IsInRole("Admin");
                if (!isAuthorized)
                {
                    return Forbid("You do not have permission to access this resource.");
                }

                // Fetch order details
                var order = await _orderService.GetOrderDetailsAsync(id, userEmail);

                // Check if the order exists
                if (order == null)
                {
                    return NotFound(new { Message = "Order not found or you do not have access to this order." });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of orders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(List<OrderSummaryDto>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden.")]
        [ProducesResponseType(404)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Error = "User email not found or token is invalid." });
                }

                var isAuthorized = User.IsInRole("User") || User.IsInRole("Admin");
                if (!isAuthorized)
                {
                    return Forbid();
                }

                // Get the orders
                var orders = await _orderService.GetAllOrdersAsync(userEmail);

                if (orders == null || !orders.Any())
                {
                    return NotFound(new { Message = "No orders found for the user." });
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(400)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        [SwaggerOperation(Summary = "Creating the order from dishes in basket")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User email not found.");
                }

                // Check if the user has the required role
                if (!User.IsInRole("User") && !User.IsInRole("Admin"))
                {
                    return Forbid("You do not have permission to create orders.");
                }

                // Validate the delivery time
                if (orderCreateDto.DeliveryTime <= DateTime.UtcNow.AddHours(1))
                {
                    return BadRequest("Delivery time must be at least 1 hour after the order time.");
                }

                // Call the order service to create the order
                var orderDto = await _orderService.CreateOrderAsync(userEmail, orderCreateDto);
                if (orderDto == null)
                {
                    return BadRequest("Failed to create order. Your basket might be empty.");
                }

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/status")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(400)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        [SwaggerOperation(Summary = "Confirm order delivery")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            try
            {
                // Check for invalid ID
                if (id == Guid.Empty)
                {
                    return BadRequest("Invalid order ID.");
                }

                // Get user email from the token
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User email not found.");
                }

                // Check if the user has the required role or permission (example check)
                if (!User.IsInRole("Admin") && !User.IsInRole("User"))
                {
                    return Forbid("You do not have permission to confirm this order.");
                }

                // Confirm the order
                var confirmed = await _orderService.ConfirmOrderAsync(id, userEmail);

                // Handle the case where the order is not found or already delivered
                if (!confirmed)
                {
                    return NotFound("Order not found or already delivered.");
                }

                return Ok(new { message = "Order confirmed successfully." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
