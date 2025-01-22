using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Delivery.Resutruant.API.Models.DTO
{
    public class DishDto
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Boolean IsVegeterian { get; set; }
        public string Photo { get; set; }
        public double Rating { get; set; }
        public string Category { get; set; } // String representation of enum
    }

}
