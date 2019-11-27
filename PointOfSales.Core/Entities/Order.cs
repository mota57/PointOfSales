using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Infraestructure.Rule;
using PointOfSales.Core.Infraestructure.TriggerHelper;
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

        public ICollection<PaymentOrder> PaymentOrders {get;set;} 

        public DiscountType DiscountType { get; set; }

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        private decimal? _customDiscountAmount = null;

        public decimal? CustomDiscountAmount {
            get {
                return _customDiscountAmount;
            }
            set {
                if(value < 0)
                {
                    _customDiscountAmount = null;
                }
                _customDiscountAmount = value;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var ctx = (POSContext) validationContext.GetService(typeof(POSContext));
            var orderRentRule = new OrderDetailRentProductsDatesAreRequired(ctx);
            var orderValidValues = new OrderValidValues(ctx);

            var handler = new SpecificationRuleHandler<Order>(orderRentRule, orderValidValues);
            handler.RunSpecifications(candidate: this);
            return handler.GetErrosForMvc();
        }
    }

    public class OrderTrigger : IBeforeCreate<Order>, IBeforeUpdate<Order>
    {
        public void BeforeCreate(DbContext context, Order newEntity)
        {
            OrderTriggerHelper.RecalculateChange(newEntity);
        }

        public void BeforeUpdate(DbContext context, Order entity)
        {
            throw new System.NotImplementedException();
        }
    }

    public class OrderTriggerHelper
    {
        public static void RecalculateChange(Order order)
        {
            var payments = order.PaymentOrders.Where(p => p.PaymentType == PaymentType.CASH);
            foreach (var payment in payments)
            {
                if (payment.Amount > payment.Due)
                {
                    payment.Change = payment.Amount - payment.Due;
                }
            }
        }
    }
}
