namespace Delivery.Resutruant.API.Models.DTO
{
    public class OrderSummaryDto
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
