using Microsoft.AspNetCore.Http;
using PointOfSales.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PointOfSales.Core.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProductCode { get; set; }

        public decimal Price { get; set; }

        public byte[] MainImage { get; set; }

        public IFormFile MainImageForm { get; set; }

        public int? CategoryId {get; set; }

        public bool ImageDeleted { get; set; }

    }
}
