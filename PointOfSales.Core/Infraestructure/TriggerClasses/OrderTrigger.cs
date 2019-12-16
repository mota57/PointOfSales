using EFTriggerHelper;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;

namespace PointOfSales.Infraestructure.TriggerClasses
{
    public class OrderTrigger : IBeforeCreate<Order>, IBeforeUpdate<Order> 
    {
        private DateTime Now = DateTime.Now;

        public OrderTrigger()
        {

        }
        public void BeforeCreate(DbContext context, IEnumerable<Order> entities)
        {
            foreach(var order in entities)
            {
                order.StatusOrder = StatusOrder.CLOSE;
                order.EnforceCorrectValuesOnDiscount();
                OrderTriggerHelper.SetDateBeforeCreate(order, Now);
                OrderTriggerHelper.EnforceRecalculateChange(order);
            }
        }

        public void BeforeUpdate(DbContext context, IEnumerable<Order> entities)
        {
            foreach(var order in entities)
            {
                order.ModifiedDate = Now;
                order.EnforceCorrectValuesOnDiscount();
                OrderTriggerHelper.SetDateBeforeUpdate(order, Now);
                OrderTriggerHelper.EnforceRecalculateChange(order);
            }
        }
    }
}
