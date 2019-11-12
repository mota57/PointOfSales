using AutoMapper;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PointOfSales.Core.Infraestructure
{
    public class POSMapperConfiguration : Profile
    {
        public POSMapperConfiguration()
        {

            ProductMapper.Configure(this);
          
            
            CreateMap<ProductModifier, ProductModifierDTO>().ReverseMap();
        }
    }

    public static class ProductMapper
    {
        public static HashSet<ProductModifier> CreateListOfProductModifier(ProductFormDTO dto)
        {
            return new HashSet<ProductModifier>(dto.ModifierIds
                 .Select(modId => new ProductModifier() { ProductId = dto.Id, ModifierId = modId }));
        }


        public static void Configure(Profile vm)
        {
            vm.CreateMap<Product, ProductFormDTO>()
              .ForMember(_ => _.ModifierIds, cfg => cfg.MapFrom(p =>
                  p.ProductModifier.Select(_ => _.ModifierId).ToList()));

            vm.CreateMap<ProductFormDTO, Product>()
                .ForMember(_ => _.MainImage, cfg => cfg.Ignore())
                .ForMember(_ => _.ProductModifier, cfg => cfg.MapFrom(dto => CreateListOfProductModifier(dto)));
        }
    }
}
