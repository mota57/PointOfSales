using System;
using System.Collections.Generic;
using PointOfSales.Core.Entities;

namespace PointOfSales.Core.DTO
{

    public class OrderForPayDTO
    {

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        public decimal? CustomDiscountAmount { get; set; }
        public IList<PaymentOrderForPayDTO> PaymentOrders {get;set;}
        
        public IList<OrderDetailForPayDTO> OrderDetails {get;set;}
    }


    public class PaymentOrderForPayDTO
    {
        public decimal Due { get; set; }
        public decimal Amount {get ;set;}
        public PaymentType  PaymentType {get;set;}
    }

    public enum DiscountTypeDTO {
        none,
        custom,
        system
    }

    public class OrderDetailForPayDTO
    {   
        public int ProductId {get;set;}

        public int Quantity {get;set;}

        private int? _discountid = null;

        public int? DiscountId {
            get {
                if(DiscountType != DiscountTypeDTO.system)
                {
                    _discountid = null;
                }
                return _discountid;
            }
            set {
                _discountid = value;
            }
        }

        public DateTime? StartDate {get;set;}
        public DateTime? EndDate {get;set;}
        public DiscountTypeDTO DiscountType {get;set;}
        public decimal? CustomDiscountAmount { get; set; }
    }
}
