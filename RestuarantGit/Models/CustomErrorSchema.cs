namespace Delivery.Resutruant.API.Models
{
    public class CustomErrorSchema
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class CanRateResponse
    {
        public bool CanRate { get; set; }
    }

}
