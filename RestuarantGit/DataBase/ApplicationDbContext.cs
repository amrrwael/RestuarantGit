using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Delivery.Resutruant.API.Models.Domain;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Delivery.Resutruant.API.DataBase
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Rating> Rate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base method first to ensure Identity configurations are applied
            base.OnModelCreating(modelBuilder);

            // Configure Dish entity
            modelBuilder.Entity<Dish>()
                .Property(d => d.Category)
                .HasConversion<string>(); // Store enum as string

            // Configure Basket entity
            modelBuilder.Entity<Basket>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<BasketItem>()
                .Property(b => b.DishId)
                .HasColumnName("DishId");


            modelBuilder.Entity<BasketItem>()
                .HasKey(bi => bi.Id);

            modelBuilder.Entity<Basket>()
                .HasMany(b => b.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
