using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    public static class HelperExtensionTest
    {
        public static void CreateProductData(this POSContext context, string name = "Prod", int total = 1)
        {
            for (int i = 0; i < total; i++)
            {
                context.Add(new Product() { Name = $"Prod{(i + 1)}" });
            }
            context.SaveChanges();
        }
    }
}
