namespace Delivery.Resutruant.API.Models.Pagination
{
    public class PagedResult<T>
    {
        public List<T> Dishes { get; set; }
        public Pagination Pagination { get; set; }
    }
}
