using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PointOfSales.Core.Entities;
using PointOfSales.WebUI.Controllers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;

namespace TestApp
{
    [TestClass]
    public class ModifierTest
    {
        private Modifier GetModiferFromClient()
        {

            return new Modifier()
            {
                Id = 1,
                Name = "mod1.1",
                ItemModifier = new HashSet<ItemModifier>()
                        {
                            new ItemModifier { Id= 1, Name = "item1.1", ModifierId = 1},
                            new ItemModifier { Id= 2, Name = "item2.1", ModifierId = 1},
                            new ItemModifier { Name = "item3.1",  ModifierId = 1} //new
                        }
            };

        }

        private Modifier BuildModifierData(string name = "mod1", int totalItemModifier = 1)
        {
            return new Modifier()
            {
                Name = name,
                ItemModifier = BuildItemModifiers(total:totalItemModifier)
            };

        }

        private List<ItemModifier> BuildItemModifiers(int total = 1)
        {
            List<ItemModifier> itemMods = new List<ItemModifier>();
            for (int i = 0; i < total; i++) {
                itemMods.Add(new ItemModifier { Name = $"item{i+1}" });
            }
            return itemMods;
        }

      

        [TestMethod]
        public void TestAddUpdateDeleteInsertRelatedEntities()
        {
            //setup
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<POSContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new POSContext(options))
                {
                    //create database
                    context.Database.EnsureCreated();
                }

                // Run the test against one instance of the context
                using (var context = new POSContext(options))
                {
                    context.Update(BuildModifierData(totalItemModifier:3));
                    context.SaveChanges();

                    var modClient = GetModiferFromClient();

                    ModifierController.UpsertDeleteModiferAndItemModifier(context, modClient);
                    context.SaveChanges();

                    Assert.AreEqual(context.Modifier.First().Name, "mod1.1");

                    Assert.AreEqual(context.ItemModifier.Count(), 3);
                    var listNameServer = context.ItemModifier
                        .Select(_ => _.Name)
                        .ToList()
                        .Aggregate((cur, next) => $"{cur};{next}");

                    var listNameClient = modClient.ItemModifier
                        .Select(_ => _.Name)
                        .ToList()
                        .Aggregate((cur, next) => $"{cur};{next}");

                    Assert.AreEqual(listNameServer, listNameClient);

                    var itemModifier3 = context.ItemModifier.Find(3);
                    Assert.AreEqual(null, itemModifier3);

                    var itemModifier4 = context.ItemModifier.Find(4);
                    Assert.IsNotNull(itemModifier4);

                }

                using (var context = new POSContext(options))
                {
                    var totalChilds = 1;
                    var modClient = BuildModifierData("hello1",totalChilds);
                    ModifierController.UpsertDeleteModiferAndItemModifier(context, modClient);
                    context.SaveChanges();
                    Assert.AreEqual(
                        context.Modifier
                        .Include(_ => _.ItemModifier)
                        .FirstOrDefault(_ => _.Name == "hello1")
                        .ItemModifier.Count(), totalChilds);

                }


            }
            finally
            {
                connection.Close();
            }
        }
    }
}
