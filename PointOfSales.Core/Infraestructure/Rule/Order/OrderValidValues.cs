using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointOfSales.Core.Infraestructure.Rule
{
    public class OrderValidValues : SpecificationRule<Order>
    {
        private POSContext ctx;

        public OrderValidValues(POSContext context)
        {
            ctx = context;

        }

     
       
        public override void Run(Order candidate)
        {
            if (candidate.OrderDetails.Any(od => od.Quantity <= 0))
                AddError($"Quantity should be at least 1", "Quantity");

            if (candidate.CustomDiscountAmount != null && candidate.CustomDiscountAmount < 0)
                AddError("Custom discount should be greater than 1", "CustomDiscountAmount");

            if (candidate.PaymentOrders.Count == 0)
                AddError("There should be at least one payment in the order", "PaymentOrders");

            if (candidate.OrderDetails.Count == 0)
                AddError("There should be at least one product in the order", "OrderDetails");

            if (candidate.PaymentOrders.Any(p => p.Amount <= 0))
                AddError("Payment Amount must be greater than 0", "PaymentOrders");
        }
    }

}
