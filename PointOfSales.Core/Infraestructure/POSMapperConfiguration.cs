using AutoMapper;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;

namespace PointOfSales.Core.Infraestructure
{
    public class POSMapperConfiguration : Profile
    {
        public POSMapperConfiguration()
        {
            CreateMap<Product, ProductFormDTO>();

            CreateMap<ProductFormDTO, Product>()
                .ForMember(_ => _.MainImage, cfg => cfg.Ignore());

            CreateMap<ProductModifier, ProductModifierDTO>().ReverseMap();

        }
    }
}
