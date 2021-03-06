﻿using PointOfSales.Core.Generator;
using PointOfSales.Core.Infraestructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PointOfSales.Core.Entities
{

    public abstract class BaseEntity 
    {

        [Required]
        [MaxLength(50)]
        public virtual string Name { get; set; }

        [Key]
        public int Id { get; set; }
        public bool SoftDetelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

  
    public class Supplier : BaseEntity
    {
        public string RNC { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    
        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Cellphone { get; set; }
    }


    public class Customer : BaseEntity
    {
        public Customer()
        {

        }

        [NotMapped]
        public override string Name { get => $"{FirstName} {LastName}";  }

        public string Identification { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        [DataType(DataType.Upload)]
        public byte[] ProfileImage { get; set; }
    }

   

    [FormLayout("", nameof(Name), nameof(Price), nameof(ProductCode), nameof(Category), nameof(MainImage) )]
    public class Product : BaseEntity
    {
        public Product() {

            ProductModifier = new HashSet<ProductModifier>();

        } 

        [MaxLength(50)]
        public string ProductCode { get; set; }
        

        [Required]
        public decimal Price { get; set; }

        [DataType(DataType.Upload), Multiple()]
        public string MainImage { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; } 

        public bool IsProductForRent {get; set;}

        public ICollection<ProductModifier> ProductModifier { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        public int? TaxId { get; set; }
        public Tax Tax { get; set; }

        public int? UnitId { get; set; }
        public Unit Unit { get; set; }

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        public decimal? DiscountAmount { get; set; }
        public DiscountType DiscountType { get;  set; } = DiscountType.None;
    }


    public class Unit : BaseEntity
    {

    }

    public class Tax : BaseEntity
    {
        public decimal Amount { get; set; }
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

    public class Category : BaseEntity
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public ICollection<Product> Products { get; set; }
    }


    public enum DiscountType
    {
        None,
        Custom,
        System
    }
    
    public class PaymentOrder 
    {
        public int PaymentOrderId {get;set;}

        public int OrderId {get; set;}
        public decimal Due { get; set; }
        public decimal Amount {get ;set;}
        public decimal? Change { get; set; }
        public PaymentType  PaymentType {get;set;}
    }

    public enum PaymentType { CASH, CARD}

    
    

   
    public class OrderAudit
    {
        public string CustomerIdentification { get; set; }
        public string CustomerName { get; set; }
        public int OrderAuditId { get; set; }

        public int ProductId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string DiscountName { get; set; }
        public decimal? DiscountAmount { get; set; }

        public string TaxName { get; set; }
        public decimal? TaxAmmount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
    }

    public class Discount : BaseEntity
    {
        [Required]
        public decimal Amount { get; set; }
    }

    

}
