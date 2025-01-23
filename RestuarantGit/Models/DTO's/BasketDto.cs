using System;
using System.Collections.Generic;

namespace Delivery.Resutruant.API.Models.DTO
{
    public class BasketDto
    {
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
    }
    public class BasketItemDto
    {
        public string name { get; set; }
        public int price { get; set; }
        public int totalPrice { get; set; }
        public int amount { get; set; }
        public string image { get; set; }
        public Guid id { get; set; }
    }
}
