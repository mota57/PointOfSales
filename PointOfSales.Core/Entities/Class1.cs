using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.Entities
{
    public static class GlobalVariables
    {
        public static  string Connection => "Data Source=pos.db";

    }

    public class BaseEntity 
    {
        public bool SoftDetelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class POSContextFactory : IDesignTimeDbContextFactory<POSContext>
    {
        public POSContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<POSContext>();
            optionsBuilder.UseSqlite(GlobalVariables.Connection);

            return new POSContext(optionsBuilder.Options);
        }
    }


    public class POSContext : DbContext 
    {

        public POSContext(DbContextOptions<POSContext> options)
        : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(GlobalVariables.Connection);
            }
        }
    }

    /*
     
#the product fields
productId
productName
productCode
productPrice
mainImage
category
unit price
*/

    public class Product 
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        
        [MaxLength(50)]
        public string ProductCode { get; set; }

        [Required]
        public decimal Price { get; set; }

        public byte[] MainImage { get; set; }

        public ICollection<Category> Categories { get; set; }
    }

    public class  Category 
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
    }

    public class OrderDetail 
    {

        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } 

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }


}
