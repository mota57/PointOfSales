using PointOfSales.Core.Entities;
using System.Linq;

namespace PointOfSales.Infraestructure.TriggerClasses
{
    public class OrderTriggerHelper
    {
        
        public static void SetDateBeforeCreate(Order order, System.DateTime now)
        {
            order.CreatedDate = now;
            order.ModifiedDate = now;
        }

        public static void SetDateBeforeUpdate(Order order, System.DateTime now)
        {
            order.ModifiedDate = now;
        }

        public static void EnforceRecalculateChange(Order order)
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
