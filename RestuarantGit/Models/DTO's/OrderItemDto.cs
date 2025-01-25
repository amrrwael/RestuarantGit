namespace Delivery.Resutruant.API.Models.DTO
{
    public class OrderItemDto
    {
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int TotalPrice { get; set; }
    }
}
