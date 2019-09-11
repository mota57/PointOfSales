using AutoMapper;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;

namespace PointOfSales.Core.Infraestructure
{
    public class POSMapperConfiguration : Profile
    {
        public POSMapperConfiguration()
        {
            CreateMap<Product, ProductDTO>();

            CreateMap<ProductDTO, Product>()
                .ForMember(_ => _.MainImage, cfg => cfg.Ignore());
        }
    }
}
