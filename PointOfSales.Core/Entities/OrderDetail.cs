using System;

namespace PointOfSales.Core.Entities
{
    public class OrderDetail  
    {   
        public OrderDetail(){
            
        }
        
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } 

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int _quantity = 1;
        public int Quantity {
            get { return _quantity;  }
            set
            {
                if(value < 0)
                {
                    _quantity = 1;
                }
                _quantity = value;

            }
        }

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        private decimal? customDiscountAmount = null;

        public decimal? CustomDiscountAmount
        {
            get
            {
                return customDiscountAmount;
            }
            set
            {
                if (value < 0)
                {
                   customDiscountAmount = null;
                }
                customDiscountAmount = value;
            }
        }

        public DateTime? StartDate {get;set;}

        public DateTime? EndDate {get;set;}


       
    }

    

}
