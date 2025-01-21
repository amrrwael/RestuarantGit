using Delivery.Resutruant.API.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace Delivery.Resutruant.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(User user, string password);
        Task<User> AuthenticateUserAsync(string email, string password);
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);

    }
}
