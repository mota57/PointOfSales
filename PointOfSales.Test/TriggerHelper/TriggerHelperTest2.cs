using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using TestApp;
using EFTriggerHelper;

namespace PointOfSales.Test.TriggerHelper
{

    /// <summary>
    /// create, update, delete
    /// </summary>
    public class PersonTrigger :
            IBeforeCreate<PersonTbl>,
            IAfterCreate<PersonTbl>,
            IBeforeUpdate<PersonTbl>,
            IAfterUpdate<PersonTbl>,
            IBeforeDelete<PersonTbl>,
            IAfterDelete<PersonTbl>
    {
        public static PersonTbl EntityAfterUpdate = null;
        public static PersonTbl EntityBeforeDelete = null;
        public static PersonTbl EntityAfterDelete = null;

        public void AfterCreate(DbContext context,  IEnumerable<PersonTbl> entities)
        {
            var ctx = (context as DummyContext);
            ctx.LogTbl.Add(new LogTbl() { Description = $"new entity added {entities.First().PersonTblId}" });
            ctx.SaveChanges();
        }


        public void BeforeCreate(DbContext context,  IEnumerable<PersonTbl> entities)
        {
            entities.First().Name = "TEST";
            entities.First().CreatedDate = DateTime.Now;
        }


        public void AfterUpdate(DbContext context,  IEnumerable<PersonTbl> entities)
        {
            EntityAfterUpdate = entities.First();
        }

        public void BeforeUpdate(DbContext context,  IEnumerable<PersonTbl> entities)
        {
            entities.First().ModifiedDate = DateTime.Now;
            entities.First().ModifiyBy = "TEST";
        }

        public void AfterDelete(DbContext context,  IEnumerable<PersonTbl> entities)
        {
            EntityBeforeDelete = entities.First();
        }

        public void BeforeDelete(DbContext context,  IEnumerable<PersonTbl> entities)
        {
            EntityAfterDelete = entities.First();
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
        public void TestBeforeAfterDeleteTrigger()
        {
            TestHandler.Handle<DummyContext>((options) =>
            {
                using (var context = new DummyContext(options))
                {
                    var p = new PersonTbl();
                    context.Add(p);
                    context.SaveChanges();
                }
                using (var context = new DummyContext(options))
                {
                    var p = context.Person.First();
                    context.Person.Remove(p);
                    context.SaveChanges();
                    Assert.AreEqual(p.PersonTblId, PersonTrigger.EntityBeforeDelete.PersonTblId);
                    Assert.AreEqual(p.PersonTblId, PersonTrigger.EntityAfterDelete.PersonTblId);
                }
            });

        }


        [TestMethod]
        public void TestBeforeAfterCreateTrigger()
        {
            TestHandler.Handle<DummyContext>((options) =>
            {
                using (var context = new DummyContext(options))
                {
                    var p = new PersonTbl();
                    context.Add(p);
                    context.SaveChanges();
                }
                using (var context = new DummyContext(options))
                {
                    var p = context.Person.First();
                    var log = context.LogTbl.First();

                    Assert.IsTrue(log.Description.StartsWith("new entity added"));
                    Assert.AreEqual("TEST", p.Name);
                    Assert.AreEqual(DateTime.Now.Year, p.CreatedDate.Value.Year);
                }
            });

        }

        [TestMethod]
        public void TestBeforeAfterUpdateTrigger()
        {
            TestHandler.Handle<DummyContext>((options) =>
            {
                using (var context = new DummyContext(options))
                {
                    var p = new PersonTbl();
                    context.Add(p);
                    context.SaveChanges();
                }
                using (var context = new DummyContext(options))
                {
                    var p = context.Person.First();
                    p.Name = "change";
                    context.SaveChanges();
                    Assert.AreEqual("TEST", p.ModifiyBy);
                    Assert.IsTrue(PersonTrigger.EntityAfterUpdate.PersonTblId == p.PersonTblId);
                }

            });

        }




    }
}
