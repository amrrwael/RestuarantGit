using System.ComponentModel.DataAnnotations;

namespace Delivery.Resutruant.API.Models.Domain
{
    public class Rating
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        [Range(1, 10)]
        public int Value { get; set; }
        public Guid DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
