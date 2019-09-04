using Microsoft.AspNetCore.Http;
using PointOfSales.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }


        [MaxLength(50)]
        public string ProductCode { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile MainImage { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
