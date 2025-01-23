using System;
using System.Collections.Generic;

namespace Delivery.Resutruant.API.Models.Domain
{
    public class Basket
    {
        public Guid Id { get; set; } // Auto-generate Basket ID
        public string UserEmail { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }

    public class BasketItem
    {
        public Guid Id { get; set; }
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; } // Price * Amount
        public int Amount { get; set; }
        public string Image { get; set; }
        public Guid BasketId { get; set; }
    }

}
