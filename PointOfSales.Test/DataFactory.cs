using PointOfSales.Core.Entities;
using System.Collections.Generic;

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

 }
