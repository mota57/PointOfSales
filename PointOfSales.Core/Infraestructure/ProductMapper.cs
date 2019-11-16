using AutoMapper;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PointOfSales.Core.Infraestructure
{
    public static class ProductMapper
    {
        public static List<ProductModifier> CreateListOfProductModifier(ProductFormDTO dto)
        {
            return  dto.ModifierIds
                 .Select(modId => new ProductModifier() { 
                        ProductId = dto.Id, 
                        ModifierId = modId 
                }).ToList();
        }


        public static void Configure(Profile vm)
        {
            vm.CreateMap<Product, ProductFormDTO>()
              .ForMember(_ => _.ModifierIds, cfg => cfg.MapFrom(p =>
                  p.ProductModifier.Select(_ => _.ModifierId).ToList()));

            vm.CreateMap<ProductFormDTO, Product>()
                .ForMember(_ => _.MainImage, cfg => cfg.Ignore());
        }
    }
}
