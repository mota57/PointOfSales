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
using PointOfSales.Core.Service;

namespace TestApp
{
    [TestClass]
    public class ModifierTest
    {
        [TestMethod]
        public void TestUpsertProductModifiers()
        {

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

                using (var context = new POSContext(options))
                {

                    context.CreateProductData();
                    context.CreateModifierData(total: 2, totalItemModifier: 2);
                    context.SaveChanges();


                    context.Add(new ProductModifier { ModifierId = 1, ProductId = 1 });
                    context.SaveChanges();

                    var productId = 1;
                    var dto = new List<ProductModifier>()
                        {
                            new ProductModifier { ProductId = productId, ModifierId = 1},
                            new ProductModifier { ProductId = productId, ModifierId = 2},
                        };

                    //assert add one more
                    var posService = new POSService(context);
                    posService.UpsertDeleteProductModifiers(productId, dto).Wait();
                    Assert.AreEqual(2, context.Product.FirstOrDefault().ProductModifier.Count());

                    //asssert remove all 
                    dto = new List<ProductModifier>() {};
                    posService.UpsertDeleteProductModifiers(productId, dto).Wait();
                    Assert.AreEqual(0, context.Product.FirstOrDefault().ProductModifier.Count());
                     
                }

            }
            finally
            {
                connection.Close();
            }
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
                    context.CreateModifierData(totalItemModifier:3);
                    context.SaveChanges();

                    var modClient = new Modifier()
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

                    var posService = new POSService(context);
                    posService.UpsertDeleteModiferAndItemModifier(modClient);

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
                    var modClient = DataFactory.BuildModifierData(name:"hello1", totalItemModifier: totalChilds);

                    var posService = new POSService(context);
                    posService.UpsertDeleteModiferAndItemModifier(modClient);


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
