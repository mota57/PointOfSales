using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Infraestructure.TriggerHelper;
using System;
using System.Linq;

namespace PointOfSales.Core.Entities
{
    public class OrderService 
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
