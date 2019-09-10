using AutoMapper;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;

namespace PointOfSales.Core.Infraestructure
{
    public class POSMapperConfiguration : Profile
    {
        public POSMapperConfiguration()
        {
            CreateMap<Product, ProductDTO>()
                .ForSourceMember(_ => _.MainImage, cfg => cfg.DoNotValidate())
                .ForMember(_ => _.MainImage, cfg => cfg.Ignore());

            CreateMap<ProductDTO, Product>()
                .ForSourceMember(_ => _.MainImage, cfg => cfg.DoNotValidate())
                .ForMember(_ => _.MainImage, cfg => cfg.Ignore());
        }
    }
}
