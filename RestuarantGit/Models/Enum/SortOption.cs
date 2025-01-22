using System.Text.Json.Serialization;

namespace Delivery.Resutruant.API.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortOption
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc,
        RatingAsc,
        RatingDesc
    }
}
