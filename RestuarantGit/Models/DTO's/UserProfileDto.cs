using System;

namespace Delivery.Resutruant.API.Models.DTO
{
    public class UserProfileDto
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Id { get; set; }
    }
}
