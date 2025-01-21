using Delivery.Resutruant.API.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Delivery.Resutruant.API.Models.Domain
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
