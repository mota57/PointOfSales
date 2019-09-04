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
        }
    }
}
