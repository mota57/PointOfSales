using PointOfSales.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointOfSales.Core.Infraestructure.Rule
{
    /// <summary>
    /// rule1: if any of the orderDetail contains a product where product.isProductRent = true and startDate or endate equal null do not allow to continue forward
    /// </summary>
    public class OrderDetailRentProductsDatesAreRequired : SpecificationRule<Order>
    {
        private POSContext ctx; 

        public OrderDetailRentProductsDatesAreRequired(POSContext context)
        {
            ctx = context;

        }

        public override void Run(Order candidate)
        {
            var productIds = candidate.OrderDetails.Select(p => p.ProductId);
            var products = ctx.Product
                           .Select(p => new Product { Id = p.Id, IsProductForRent = p.IsProductForRent })
                           .Where(p => productIds.Contains(p.Id));


            foreach (var orderD in candidate.OrderDetails)
            {
                var product = products.First(p => p.Id == orderD.ProductId);
                if (product.IsProductForRent && (orderD.StartDate == null || orderD.EndDate == null))
                {
                    AddError($"Start date and end date are required for product {product.Id}-{product.Name}");
                }
            }

        }
    }
}
