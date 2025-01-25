using Delivery.Resutruant.API.Models;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Models.Enums;
using Delivery.Resutruant.API.Models.Pagination;
using Delivery.Resutruant.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Delivery.Resutruant.API.Controllers
{
    [Route("api/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        private readonly IRateService _rateService;

        public DishController(IDishService dishService, IRateService rateService)
        {
            _dishService = dishService;
            _rateService = rateService;
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

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(summary: "Get a list of dishes (menu)")]
        [ProducesResponseType(typeof(PagedResult<DishDto>), 200)]
        [ProducesResponseType(400)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        public async Task<IActionResult> GetAllDishes([FromQuery] Category? category = null,
            [FromQuery] bool? vegetarian = false,
            [FromQuery] SortOption sorting = SortOption.NameAsc,
            [FromQuery] int page = 1)
        {
            try
            {
                // Validate the page number
                if (page < 1)
                {
                    return BadRequest(new CustomErrorSchema
                    {
                        Status = "error",
                        Message = "Page number must be greater than or equal to 1."
                    });
                }

                // Retrieve paginated dishes
                var pagedDishes = await _dishService.GetAllDishesAsync(category, vegetarian, sorting, page);
                return Ok(pagedDishes);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id:Guid}")]
        [SwaggerOperation(summary: "Get information about concrete dish")]
        [ProducesResponseType(typeof(DishDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CustomErrorSchema))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var dish = await _dishService.GetDishByIdAsync(id);
                if (dish == null)
                    return NotFound();

                return Ok(dish);

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{dishId:Guid}/rating/check")]
        [SwaggerOperation(summary: "Check if user is able to set rating for the dish")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        public async Task<IActionResult> CanRateDishAsync(Guid dishId)
        {
            try
            {
                // Retrieve user email from the token
                var userEmail = GetUserEmailFromToken();
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Error = "User email could not be retrieved from token." });
                }

                // Check if the dish exists
                var dishExists = await _dishService.GetDishByIdAsync(dishId);
                if (dishExists == null)
                {
                    return NotFound(new { Message = "Dish not found." });
                }

                // Check if the user is authorized to rate this dish (optional, based on your app's rules)
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                if (userRole != "User")
                {
                    return Forbid("Only regular users can rate dishes.");
                }

                // Check if the user can rate the dish
                var canRate = await _rateService.CanRateDishAsync(userEmail, dishId);
                return Ok(canRate);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("{dishId:Guid}/rating/{rate:int}")]
        [SwaggerOperation(summary: "Set a rating for a dish")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        public async Task<IActionResult> RateDishAsync(Guid dishId, int rate)
        {
            if (rate < 1 || rate > 10)
            {
                return BadRequest("Rating must be between 1 and 10.");
            }

            try
            {
                var userEmail = GetUserEmailFromToken();
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User email could not be retrieved from token.");
                }

                var dishExists = await _dishService.GetDishByIdAsync(dishId);
                if (dishExists == null)
                {
                    return NotFound("Dish not found.");
                }

                var canRate = await _rateService.CanRateDishAsync(userEmail, dishId);
                if (!canRate)
                {
                    return BadRequest("You can only rate dishes from orders that you have received.");
                }

                await _rateService.RateDishAsync(userEmail, dishId, rate);
                return Ok("Rating submitted successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        private string GetUserEmailFromToken()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
