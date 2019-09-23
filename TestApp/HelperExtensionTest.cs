using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    public static class DataFactory
    {
        public static Modifier BuildModifierData(string name = "mod1", int totalItemModifier = 1)
        {
            return BuildModifiersData(name, total:1, totalItemModifier: totalItemModifier)[0];
        }

        public static List<Modifier> BuildModifiersData(string name= "mod1", int total= 1, int totalItemModifier =1)
        {
            List<Modifier> modifiers = new List<Modifier>();
            for (int j = 0; j < total; j++)
            {
                modifiers.Add(new Modifier() {
                    Name = name,
                    ItemModifier = BuildItemModifiers(total: totalItemModifier)
                });
            }
            return modifiers;

        }

        public static List<ItemModifier> BuildItemModifiers(int total = 1)
        {
            var itemMods = new List<ItemModifier>();
            for (int i = 0; i < total; i++)
            {
                itemMods.Add(new ItemModifier { Name = $"item{i + 1}" });
            }
            return itemMods;
        }
    }

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
