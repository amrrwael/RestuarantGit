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

    public async Task<User> AuthenticateUserAsync(string email, string password)
    {
        // Retrieve the user by email.
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
            // Attempt to sign in the user with the provided password.
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                return user; // Return the authenticated user if sign-in is successful.
            }
        }

        return null; // Return null if authentication fails.
    }
    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _context.Users.FindAsync(userId);
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        return await _userManager.UpdateAsync(user);
    }

}
