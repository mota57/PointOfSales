using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PointOfSales.Core.Infraestructure.Specification
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);

        ISpecification<T> And(ISpecification<T> other);

        ISpecification<T> Not();
    }


    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private ISpecification<T> _leftSpecification;
        private ISpecification<T> _rightSpecification;

        public AndSpecification(ISpecification<T> leftSpecification, ISpecification<T> rightSpecification)
        {
            _leftSpecification = leftSpecification;
            _rightSpecification = rightSpecification;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return _leftSpecification.IsSatisfiedBy(candidate) && _rightSpecification.IsSatisfiedBy(candidate);
        }
    }


    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        protected List<ValidationResult> ErrorList = new List<ValidationResult>();

        public abstract bool IsSatisfiedBy(T candidate);

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        public IEnumerable<ValidationResult> GetMessageErrors()
        {
            var tempValues = ErrorList.ToList();
            ErrorList.Clear();
            return tempValues;
        }


    }


    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private ISpecification<T> _innerSpecification;

        public NotSpecification(ISpecification<T> innerSpecification)
        {
            _innerSpecification = innerSpecification;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return !_innerSpecification.IsSatisfiedBy(candidate);
        }
    }
    public class OrderValidValues : CompositeSpecification<Order>
    {
        private POSContext ctx;

        public OrderValidValues(POSContext context)
        {
            ctx = context;

        }

        public override bool IsSatisfiedBy(Order candidate)
        {
            if(candidate.OrderDetails.Any(od => od.Quantity <= 0))
            {
                ErrorList.Add(
                       new ValidationResult(
                       $"Quantity should be at least 1",
                       new[] { "" })
                   );

            }

            if(candidate.CustomDiscountAmount  != null && candidate.CustomDiscountAmount < 0)
            {
                ErrorList.Add(
                       new ValidationResult(
                       $"Custom discount should be greater than 1",
                       new[] { "" })
                   );
            }

            if(candidate.PaymentOrders.Count == 0)
            {
                ErrorList.Add(
                       new ValidationResult(
                       $"There should be at least one payment in the order",
                       new[] { "" })
                   );
            }


            if (candidate.OrderDetails.Count == 0)
            {
                ErrorList.Add(
                       new ValidationResult(
                       $"There should be at least one product in the order",
                       new[] { "" })
                   );
            }

            if (candidate.PaymentOrders.Any(p => p.Amount <= 0))
            {
                ErrorList.Add(
                       new ValidationResult(
                       $"Payment Amount must be greater than 0",
                       new[] { "" })
                   );
            }

            return ErrorList.Count == 0;
        }
    }



    /// <summary>
    /// rule1: if any of the orderDetail contains a product where product.isProductRent = true and startDate or endate equal null do not allow to continue forward
    /// </summary>
    public class OrderDetailRentProductsDatesAreRequired : CompositeSpecification<Order>
    {
        private POSContext ctx; 

        public OrderDetailRentProductsDatesAreRequired(POSContext context)
        {
            ctx = context;

        }

        public override bool IsSatisfiedBy(Order candidate)
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
                    ErrorList.Add(
                        new ValidationResult(
                        $"Start date and end date are required for product {product.Id}-{product.Name}, ",
                        new[] { "" })
                    );
                }
            }

            return ErrorList.Count == 0;
        }
    }
}
