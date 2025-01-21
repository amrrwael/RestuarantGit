using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Repositories.Interfaces;
using Delivery.Resutruant.API.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Constructor to initialize dependencies via dependency injection
public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ApplicationDbContext _context;

    public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    // Registers a new user with the given credentials and initializes their basket.
    public async Task<IdentityResult> RegisterUserAsync(User user, string password)
    {
        // Create a new user in the database.
        var result = await _userManager.CreateAsync(user, password);
        return result;
    }

}
