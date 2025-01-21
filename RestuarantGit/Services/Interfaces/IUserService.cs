using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.DTO;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Delivery.Resutruant.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(User user, string password);
        Task<string> LoginUserAsync(string email, string password);
        Task<UserProfileDto> GetUserProfileAsync(string userId);

    }
}
