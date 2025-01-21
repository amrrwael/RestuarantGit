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
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [SwaggerOperation(summary: "Log in to the system")]
        public async Task<IActionResult> Login([FromBody] LoginDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Return validation errors
                    return BadRequest(ModelState);
                }

                var token = await _userService.LoginUserAsync(userDto.Email, userDto.Password);
                if (token != null)
                {
                    return Ok(new TokenResponseDto { Token = token });
                }

                // Return bad request if login fails
                return BadRequest(new { Message = "Invalid email or password." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpPost("logout")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        [SwaggerOperation(summary: "Log out system user")]
        public async Task<IActionResult> Logout([FromServices] IBlacklistService blacklistService)
        {
            try
            {
                // Ensure the Authorization header exists
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return BadRequest(new { error = "Authorization header is missing." });
                }

                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

                // Validate the token format
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { error = "Invalid token format." });
                }

                // Simulate a case where the token is invalid or unauthorized
                if (!User.Identity?.IsAuthenticated ?? false)
                {
                    return Unauthorized(new { error = "User is not authenticated." });
                }

                if (!User.IsInRole("User") && !User.IsInRole("Admin"))
                {
                    return Forbid("You do not have permission to perform this action.");
                }

                // Simulate a database or service failure
                if (blacklistService == null)
                {
                    return StatusCode(500, new { error = "Blacklist service is unavailable." });
                }

                // Get token expiration (assuming tokens expire in 60 minutes, adjust accordingly)
                var expiration = DateTime.UtcNow.AddMinutes(60);

                // Add the token to the blacklist
                await blacklistService.AddToBlacklistAsync(token, expiration);

                return Ok(new { message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet("profile")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(UserProfileDto))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "InternalServerError", typeof(CustomErrorSchema))]
        [Authorize]
        [SwaggerOperation(summary: "Get user profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get user ID from the token
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Check if user has the required role (User or Admin)
                if (!User.IsInRole("User") && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var userProfile = await _userService.GetUserProfileAsync(userId);
                if (userProfile == null)
                {
                    return NotFound();
                }

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


    }
}
