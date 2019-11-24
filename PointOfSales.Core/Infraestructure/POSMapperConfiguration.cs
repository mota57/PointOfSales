using AutoMapper;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;

namespace PointOfSales.Core.Infraestructure
{
    public class POSMapperConfiguration : Profile
    {
        public POSMapperConfiguration()
        {
            CreateMap<OrderForPayDTO, Order>();
            CreateMap<OrderDetailForPayDTO, OrderDetail>();
            CreateMap<PaymentOrderForPayDTO, PaymentOrder>();
            ProductMapper.Configure(this);
          
          
            // CreateMap<ProductRentDetail, ProductRentDetail>();
            
            CreateMap<ProductModifier, ProductModifierDTO>().ReverseMap();
        
        }
    }
}
