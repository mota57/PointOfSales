using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.Entities
{

    public abstract class BaseEntity 
    {

        [Required]
        [MaxLength(50)]
        public virtual string Name { get; set; }

        [Key]
        public int Id { get; set; }
        //public bool SoftDetelete { get; set; }
        //public DateTime CreateDate { get; set; }
        //public DateTime ModifiedDate { get; set; }
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

    public class Product : BaseEntity
    {
        public Product() {
            ProductModifier = new HashSet<ProductModifier>();

        } 

        [MaxLength(50)]
        public string ProductCode { get; set; }

        [Required]
        public decimal Price { get; set; }

        public byte[] MainImage { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; } 

        public ICollection<ProductModifier> ProductModifier { get; set; }

    }
    public class ProductModifier
    {
        public int ProductId { get; set; }
        public Modifier Modifier { get; internal set; }
        public int ModifierId { get; set; }
        public Product Product { get; internal set; }
    }

    public class Modifier : BaseEntity
    {
        public Modifier()
        {
            ItemModifier = new HashSet<ItemModifier>();
            ProductModifier = new HashSet<ProductModifier>();
        }

        public ICollection<ItemModifier> ItemModifier { get; set; }
        public ICollection<ProductModifier> ProductModifier { get; set; }

    }

    public class ItemModifier  : BaseEntity
    {
        public int ModifierId { get; set; }
        public Modifier Modifier { get; set; } 
        public decimal Price { get; set; }
    }

    public class  Category : BaseEntity
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public ICollection<Product> Products { get; set; }
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
