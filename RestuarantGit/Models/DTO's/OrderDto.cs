namespace Delivery.Resutruant.API.Models.DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string Address { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
