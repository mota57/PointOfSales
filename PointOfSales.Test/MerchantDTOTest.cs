using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSales.Core.Infraestructure;
using PointOfSales.Core.DTO;
using PointOfSales.Core.Entities;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Newtonsoft.Json;

namespace TestApp
{
    [TestClass]
    public class MerchanTest
    {
        
        [TestMethod]
        public void TestCanMapDTOToEntities() {

            var config = new MapperConfiguration(cfg => cfg.AddProfile(typeof(POSMapperConfiguration))); 
            var mapper = config.CreateMapper();
            var odDTO = new OrderDetailForPayDTO(){
                        DiscountType = DiscountTypeDTO.system,
                        DiscountId = 1,
                        ProductId = 1,
                        Quantity = 1,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(3),
                        CustomDiscountAmount = 400,
                    }; 
            var paymentOrder = new PaymentOrderForPayDTO{
                Amount = 50,
                Due = 50,
                PaymentType = PaymentType.CARD
            };
                    
            var source = new OrderForPayDTO(){
                OrderDetails = new List<OrderDetailForPayDTO>(){ odDTO },
                PaymentOrders = new List<PaymentOrderForPayDTO>() { paymentOrder }
            };


            var order = mapper.Map<Order>(source);
            var orderDetailDTO = order.OrderDetails.First();
            var paymentOrderDTO = order.PaymentOrders.First();
            
            
            Logger.LogMessage("order {0}", JsonConvert.SerializeObject(order, Formatting.Indented));

      
            Logger.LogMessage("order detail DTO {0}", JsonConvert.SerializeObject(odDTO, Formatting.Indented));

            Logger.LogMessage("payment order DTO {0}", JsonConvert.SerializeObject(paymentOrderDTO, Formatting.Indented));

            Assert.AreEqual(odDTO.ProductId, orderDetailDTO.ProductId);
            Assert.AreEqual(odDTO.CustomDiscountAmount, orderDetailDTO.CustomDiscountAmount );
            Assert.AreEqual(odDTO.DiscountId, orderDetailDTO.DiscountId);
            Assert.AreEqual(odDTO.Quantity, orderDetailDTO.Quantity );
            Assert.AreEqual(odDTO.StartDate, orderDetailDTO.StartDate );
            Assert.AreEqual(odDTO.EndDate, orderDetailDTO.EndDate );

            Assert.AreEqual(paymentOrder.Amount, paymentOrderDTO.Amount);
            Assert.AreEqual(paymentOrder.Due, paymentOrderDTO.Due);
            Assert.AreEqual(paymentOrder.PaymentType, paymentOrderDTO.PaymentType);
        }
    }
}
