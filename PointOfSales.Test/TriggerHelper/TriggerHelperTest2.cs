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
using TestApp;

namespace PointOfSales.Test.TriggerHelper
{

    /// <summary>
    /// create, update, delete
    /// </summary>
    public class PersonTrigger : IBeforeCreate<PersonTbl>, IAfterCreate<PersonTbl>, IBeforeUpdate<PersonTbl>
    {
        public void AfterCreate(DbContext context, PersonTbl entity)
        {
            var ctx = (context as DummyContext);
            ctx.LogTbl.Add(new LogTbl() { Description = $"new entity added {entity.PersonTblId}" });
            ctx.SaveChanges();
        }

        public void BeforeCreate(DbContext context, PersonTbl entity)
        {
            entity.Name = "TEST";
            entity.CreatedDate = DateTime.Now;
        }

        public void BeforeUpdate(DbContext context, PersonTbl entity)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.ModifiyBy = "TEST";
        }
    }


    [TestClass]
    public class TriggerHelperTest2
    {
        [TestMethod]
        public void TestCanGetConcreteTypes()
        {

            var assembly = typeof(PersonTrigger).Assembly;

            DbContextTriggerHelper helper = new DbContextTriggerHelper(assembly);

            IEnumerable<MetaGenericType> types = helper.GetTypesWithInferfaceOfType(typeof(IBeforeCreate<>));

            Assert.IsTrue(types.Any(p => p.Implementor == typeof(PersonTrigger) && p.EntityTypeArg == typeof(PersonTbl)));
        }


        [TestMethod]
        public void TestCanExecuteTrigger()
        {

            TestHandler.Handle<DummyContext>((options) => {
                using (var context = new DummyContext(options))
                {
                    var p = new PersonTbl();
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

                    p.Name = "change";
                    context.SaveChanges();

                }
                using (var context = new DummyContext(options))
                {

                    var p = context.Person.First();

                    Assert.AreEqual("TEST", p.ModifiyBy);
                    Assert.IsTrue(p.ModifiedDate != null && p.ModifiedDate.Value.Year == DateTime.Now.Year);
                }

            });

        }
        



    }
}
