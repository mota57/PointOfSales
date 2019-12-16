using PointOfSales.Core.Infraestructure.Rule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointOfSales.Core.Entities
{
    public class Order : IValidatableObject
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            PaymentOrders = new HashSet<PaymentOrder>();
            StatusOrder = StatusOrder.OPEN;
        }

        public StatusOrder StatusOrder { get; set; }


        public int OrderId { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<PaymentOrder> PaymentOrders { get; set; }

        public DiscountType DiscountType { get; set; }

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        private decimal? _customDiscountAmount = null;

        public decimal? CustomDiscountAmount {
            get {
                return _customDiscountAmount;
            }
            set {
                if (value < 0)
                {
                    _customDiscountAmount = null;
                }
                _customDiscountAmount = value;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var ctx = (POSContext)validationContext.GetService(typeof(POSContext));
            var orderRentRule = new OrderDetailRentProductsDatesAreRequired(ctx);
            var orderValidValues = new OrderValidValues(ctx);

            var handler = new SpecificationRuleHandler<Order>(orderRentRule, orderValidValues);
            handler.RunSpecifications(candidate: this);
            return handler.GetErrosForMvc();
        }

        public void EnforceCorrectValuesOnDiscount()
        {
            if (DiscountType == DiscountType.None)
            {
                this.DiscountId = null;
                this.CustomDiscountAmount = null;
            }

            if (DiscountType == DiscountType.Custom)
                this.DiscountId = null;

            if (DiscountType == DiscountType.System)
                this.CustomDiscountAmount = null;
        }
    }

   
}
