using System.Security.Claims;
using Delivery.Resutruant.API.Configrations;
using Delivery.Resutruant.API.Models;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Delivery.Restaurant.API.Controllers
{

    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        private string GetUserEmail()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new UnauthorizedAccessException("User email not found.");
            }
            return userEmail;
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



        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(List<BasketItemDto>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        [SwaggerOperation(summary: "Get user cart")]
        public async Task<IActionResult> GetUserCart()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { message = "User email not found." });
                }

                // Check if the user is authorized to access the cart
                if (!User.IsInRole("Admin") && !User.IsInRole("User"))
                {
                    return Forbid("You are not authorized to access this cart.");
                }

                var cartDto = await _basketService.GetUserBasketAsync(userEmail);

                return Ok(cartDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpPost("dish/{dishId}")]
        [Authorize]
        [SwaggerOperation(summary: "Add dish to cart", Description = "Adds a dish to the user's cart.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        public async Task<IActionResult> AddDishToCart(Guid dishId)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { message = "Unauthorized access. User email not found." });
                }

                // Check if the user has the appropriate role (optional)
                if (!User.IsInRole("Admin") && !User.IsInRole("User"))
                {
                    return Forbid("You are not authorized to add dishes to the cart.");
                }

                var result = await _basketService.AddDishToBasketAsync(userEmail, dishId);

                if (!result)
                {
                    return NotFound(new { message = "Dish not found." });
                }

                return Ok(new { message = "Dish added to the basket successfully." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpDelete("dish/{dishId}")]
        [Authorize]
        [SwaggerOperation(summary: "Remove dish from cart", Description = "Removes a dish from the user's cart or decreases its quantity.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        public async Task<IActionResult> RemoveDishFromCart(Guid dishId, bool increase = false)
        {
            try
            {
                var userEmail = GetUserEmail();
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { message = "Unauthorized access. User email not found." });
                }

                // Check if the user has the appropriate role (optional)
                if (!User.IsInRole("User"))
                {
                    return Forbid("You are not authorized to remove dishes from the cart.");
                }

                var result = await _basketService.RemoveDishFromBasketAsync(userEmail, dishId, increase);

                if (!result)
                {
                    return NotFound(new { message = "Dish not found in the basket." });
                }

                var message = increase
                    ? "Dish quantity decreased by 1."
                    : "Dish removed from the basket.";
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


    }
}
