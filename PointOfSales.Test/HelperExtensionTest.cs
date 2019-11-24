using PointOfSales.Core.Entities;
using System;
using System.Text;

namespace TestApp
{
    

    public static class HelperExtensionTest
    {
        public static void CreateProductData(this POSContext context,  int total = 1)
        {
            for (int i = 0; i < total; i++)
            {
                context.Add(new Product() { Name = $"Prod{(i + 1)}" });
            }
            context.SaveChanges();
        }

        public static void CreateModifierData(this POSContext context,  int total = 1, int totalItemModifier = 1)
        {
            context.AddRange(DataFactory.BuildModifiersData(total: total, totalItemModifier: totalItemModifier));
            context.SaveChanges();
        }
    }

 }
