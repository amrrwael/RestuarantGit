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

    public async Task<string> LoginUserAsync(string email, string password)
    {
        var user = await _userRepository.AuthenticateUserAsync(email, password);
        if (user == null) return null;

        var claims = await GetClaimsForUserAsync(user);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<IList<Claim>> GetClaimsForUserAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Adding the 'jti' claim

        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }


}
