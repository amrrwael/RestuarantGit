using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;

namespace Delivery.Resutruant.API.Models.Domain
{
    public enum Category
    {
        [EnumMember(Value = "WOK")]
        WOK,

        [EnumMember(Value = "Pizza")]
        Pizza,

        [EnumMember(Value = "Soup")]
        Soup,

        [EnumMember(Value = "Desert")]
        Desert,

        [EnumMember(Value = "Drink")]
        Drink
    }
    public class Dish
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Boolean IsVegeterian { get; set; }
        public string Photo { get; set; }
        public Category Category { get; set; }
        public double? Rating{ get; set; }

    }
}
