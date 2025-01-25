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

        
    }
}
