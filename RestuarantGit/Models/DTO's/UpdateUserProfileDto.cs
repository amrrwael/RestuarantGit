using Delivery.Resutruant.API.Models.Enums;
using System;

namespace Delivery.Resutruant.API.Models.DTO
{
    public class UpdateUserProfileDto
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
