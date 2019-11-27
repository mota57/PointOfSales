//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using PointOfSales.Core.Infraestructure.TriggerHelper;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using TestApp;
//using System.Linq;
//using PointOfSales.Core.Extensions;

//namespace PointOfSales.Test
//{
//    [TestClass]
//    public class TriggerHelperTest
//    {
//        public class DummyContext : DbContext
//        {
//            public DbSet<Person> People { get; set; }

//            public DummyContext(DbContextOptions<DummyContext> options)
//                : base(options)
//            { }


//            DbContextTriggerHelper helper = new DbContextTriggerHelper();

//            public override int SaveChanges()
//            {
//                helper.BeforeCreate(this);
//                return base.SaveChanges();
//            }

//            public override int SaveChanges(bool acceptAllChangesOnSuccess)
//            {
//                helper.BeforeCreate(this);
//                return base.SaveChanges(acceptAllChangesOnSuccess);
//            }

//            public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
//            {
//                await helper.BeforeCreateAsync(this);
//                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
//            }

//            public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
//            {
//                await helper.BeforeCreateAsync(this);
//                return await base.SaveChangesAsync(cancellationToken);
//            }

//        }

//        public class Person : IBeforeCreate
//        {
//            public int PersonId { get; set; }
//            public string Name { get; set; }
//            public DateTime? CreatedDate { get; set; }

//            public void BeforeCreate(DbContext context)
//            {
//                this.Name = "TEST";
//                this.CreatedDate = DateTime.Now;
//            }
//        }

//        [TestMethod]
//        public void TestBeforeCreateSync()
//        {
//            TestHandler.Handle((DbContextOptions<DummyContext> options) =>
//            {
//                var pClient = new Person();
//                using (var ctx = new DummyContext(options))
//                {
//                    ctx.Add(pClient);
//                    ctx.SaveChanges();
//                }

//                Assert.IsTrue(pClient.Name.EqualIgnoreCase("TEST"));
//                Assert.IsNotNull(pClient.CreatedDate);

//                using (var ctx = new DummyContext(options))
//                {
//                    var personFromDb = ctx.People.First();
//                    Assert.IsTrue(personFromDb.Name.EqualIgnoreCase("TEST"));
//                    Assert.IsNotNull(personFromDb.CreatedDate);
//                }

//            });
//        }
//    }
//}
