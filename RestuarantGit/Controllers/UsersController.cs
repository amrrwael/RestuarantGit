using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Delivery.Resutruant.API.Models.DTO;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Delivery.Resutruant.API.Models;


namespace Delivery.Resutruant.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [SwaggerOperation(summary: "Register a new user")]
        public async Task<IActionResult> Register([FromBody] RegisterDto userDto)
        {
            try
            { 
                if (!ModelState.IsValid)
                {
                // Return validation errors
                return BadRequest(ModelState);
                }

                var user = _mapper.Map<User>(userDto);
                var result = await _userService.RegisterUserAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                // Return registration errors
                 return BadRequest(new { Errors = result.Errors });
                }

                var token = await _userService.LoginUserAsync(userDto.Email, userDto.Password);
                return Ok(new TokenResponseDto { Token = token });
                }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

       
    }
}
