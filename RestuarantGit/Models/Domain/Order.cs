namespace Delivery.Resutruant.API.Models.Domain
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserEmail { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        public DateTime DeliveryTime { get; set; }
        public string Address { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; } = "InProcess";
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int TotalPrice { get; set; }  
        public Order Order { get; set; }
        public Dish Dish { get; set; }
    }
}
