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

    public class ModifierDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ItemModifierDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int ModifierId { get; set; }
        public decimal Price { get; set; }
    }


    public class ProductModifierDTO
    {
        public int ProductId { get; set; }
        public int ModifierId { get; set; }
    }

    public class ProductFormDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string ProductCode { get; set; }

        [Required]
        public decimal Price { get; set; }

        //ImageBytesMainImage
        public byte[] MainImage { get; set; }

        public IFormFile MainImageForm { get; set; }

        public int? CategoryId {get; set; }

        public bool ImageDeleted { get; set; }

        public List<int> AttributeIds { get; set; }

        public List<int> ModifierIds { get; set; } = new List<int>();


    }
}
