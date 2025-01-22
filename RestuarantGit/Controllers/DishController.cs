using Delivery.Resutruant.API.Models;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Models.Enums;
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

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
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
        [ProducesResponseType(typeof(<DishDto>), 200)]
        [ProducesResponseType(400)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        public async Task<IActionResult> GetAllDishes([FromQuery] Category? category = null,
            [FromQuery] bool? vegetarian = false,
        {
            try
            {
                var Dishes = await _dishService.GetAllDishesAsync(category, vegetarian);
                return Ok(Dishes);
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
            try { 
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

        private string GetUserEmailFromToken()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
