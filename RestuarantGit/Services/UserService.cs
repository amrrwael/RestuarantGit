using AutoMapper;
using Delivery.Resutruant.API.Configurations;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Repositories.Interfaces;
using Delivery.Resutruant.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    // Constructor to initialize dependencies via dependency injection
    public UserService(IUserRepository userRepository, IConfiguration configuration, UserManager<User> userManager, IMapper mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _userManager = userManager;
        _mapper = mapper;
        _jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();
    }

    // Registers a new user and assigns the "User" role.
    public async Task<IdentityResult> RegisterUserAsync(User user, string password)
    {
        var result = await _userRepository.RegisterUserAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        return result;
    }


  
   
}
