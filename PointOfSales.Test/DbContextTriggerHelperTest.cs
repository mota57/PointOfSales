using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSales.Core.Infraestructure.EFTriggerHelper;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace PointOfSales.Test
{

    public class Person 
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
    public class PersonTrigger : IBeforeCreate<Person>
    {
        public void BeforeCreate(DbContext context, Person entity)
        {
            entity.Name = "TEST";
            entity.CreatedDate = DateTime.Now;
        }
    }

    public class DummyContext : DbContext
    {
        public DbSet<Person> Person { get; set; }

        public DummyContext(DbContextOptions<DummyContext> options)
            : base(options)
        {
            helper = new DbContextTriggerHelper(typeof(PersonTrigger).Assembly);
        }


        DbContextTriggerHelper helper { get; set; }

        public override int SaveChanges()
        {
            helper.BeforeCreate(this);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            helper.BeforeCreate(this);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        //public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    await helper.BeforeCreateAsync(this);
        //    return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    await helper.BeforeCreateAsync(this);
        //    return await base.SaveChangesAsync(cancellationToken);
        //}

    }



    [TestClass]
    public class DbContextTriggerHelperTest
    {
        //[TestMethod]
        //public void TestCanGetConcreteTypes()
        //{



        //    DbContextTriggerHelper helper = new DbContextTriggerHelper();
        //    var assembly = typeof(PointOfSales.Test.PersonTrigger).Assembly;
        //    var types = helper.GetTypesWithInferfaceOfType(
        //       assembly, typeof(IBeforeCreate<>));
        //    Assert.IsTrue(types.Any(p => p == typeof(PersonTrigger)));
        //}


        [TestMethod]
        public void TestCanExecuteTrigger()
        {

            DbContextTriggerHelper helper = new DbContextTriggerHelper();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DummyContext>()
               .UseSqlite(connection)
               .Options;

            using (var context = new DummyContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new DummyContext(options))
            {
                var p = new Person();
                context.Add(p);
                context.SaveChanges();
                Assert.AreEqual(p.Name, "TEST");
                Assert.AreEqual(p.CreatedDate.Value.Year, DateTime.Now.Year);
            }
            using (var context = new DummyContext(options))
            {
                var p = context.Person.First();
                Assert.AreEqual("TEST", p.Name);
                Assert.AreEqual(DateTime.Now.Year, p.CreatedDate.Value.Year);
            }

        }
        



    }
}
