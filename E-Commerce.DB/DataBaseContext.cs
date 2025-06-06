﻿using E_Commerce.DB.Models;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DB
{
    public class DataBaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Video> Videos { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Product>().HasData(new Product
        //    {
        //        Id = 1,
        //        Name = "Laptop",
        //        Description = "Gaming laptop",
        //        Price = 1500,
        //        Stock = 10,
        //        Brand = "Dell",
        //        //CategoryId = 1,
        //        Images = null,
        //        Viedos = null,
        //        IsDeleted = false
        //    });
        //}
    }
}
