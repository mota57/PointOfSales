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
using PointOfSales.Core.Infraestructure;

namespace TestApp
{
    public class TestHandler {
        public static void Handle(Action<DbContextOptions<POSContext>> action)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            
            var options = new DbContextOptionsBuilder<POSContext>()
                .UseSqlite(connection)
                .Options;
            
            try
            {
                using (var context = new POSContext(options))
                {
                    //create database
                    context.Database.EnsureCreated();
                }
                action(options);
            
            } finally
            {
                connection.Close();
            }
        }
    }


    [TestClass]
    public class ModifierTest
    {
        [TestMethod]
        public void TestUpsertProductModifiers()
        {
            TestHandler.Handle((DbContextOptions<POSContext> options) => {

                var productId = 1;
              
                using (var context = new POSContext(options))
                {

                    context.CreateProductData();
                    context.CreateModifierData(total: 2, totalItemModifier: 2);
                    context.SaveChanges();


                    context.Add(new ProductModifier { ModifierId = 1, ProductId = 1 });
                    context.SaveChanges();
              
                
                    var dto = new List<ProductModifier>()
                        {
                            new ProductModifier { ProductId = productId, ModifierId = 1},
                            new ProductModifier { ProductId = productId, ModifierId = 2},
                        };

                    //assert add one more
                    var productService = new ProductService(context);
                    var product = productService.GetProduct(productId).Result;
                    productService.UpsertDeleteProductModifiers(product, dto);
                    context.SaveChanges();

                    Assert.AreEqual(2, context.Product.FirstOrDefault().ProductModifier.Count());
                }

                using (var context = new POSContext(options))
                {
                    //asssert remove all 
                    var dto = new List<ProductModifier>() {};
                    var productService = new ProductService(context);
                    var product2 = productService.GetProduct(productId).Result;
                    productService.UpsertDeleteProductModifiers(product2, dto);
                    context.SaveChanges();

                    Assert.AreEqual(0, context.Product.FirstOrDefault().ProductModifier.Count());
                }   
            });
         
        }

      

        [TestMethod]
        public void TestAddUpdateDeleteInsertRelatedEntities()
        {
             TestHandler.Handle((DbContextOptions<POSContext> options) => {
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

                    var modifierService = new ModifierService(context);
                    modifierService.UpsertDeleteModiferAndItemModifier(modClient);
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
                    var modClient = DataFactory.BuildModifierData(name:"hello1", totalItemModifier: totalChilds);

                    var modifierService = new ModifierService(context);
                    modifierService.UpsertDeleteModiferAndItemModifier(modClient);
                    context.SaveChanges();

                    Assert.AreEqual(
                        context.Modifier
                        .Include(_ => _.ItemModifier)
                        .FirstOrDefault(_ => _.Name == "hello1")
                        .ItemModifier.Count(), totalChilds);
                }


            });
          
        }
    }
}
