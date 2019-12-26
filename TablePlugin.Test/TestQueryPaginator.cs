using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using TablePlugin.Core;
using Newtonsoft.Json;
using TablePlugin.Data;

namespace TablePlugin.Test
{
    [TestClass]
    public class TestQueryPaginator
    {
        [TestMethod]
        public void TestFilterOperators()
        {
            TestHandler.Handle<DummyContext>((options) => {
                
                using(var ctx = new DummyContext(options))
                {
                    ctx.Person.AddRange(
                        new Person() { FirstName = "Carlos", LastName = "Sank", Age = 23},
                        new Person() { FirstName = "Mario", LastName = "Sank", Age = 40},
                        new Person() { FirstName = "Carla", LastName = "Sank", Age = 11}
                    );
                }
                //test startwith
                TablePluginOptions.DatabaseProvider = DatabaseProvider.SQLite;
                TablePluginOptions.SQLConnectionName = "DataSource=:memory";
                var query=  new QueryPaginator();
                var queryconfig = new QueryConfig("Person", 
                                 new QueryField("FirstName"),
                                 new QueryField("LastName"),
                                 new QueryField(name:"Age", type:"integer"));

                var result = query.GetAsync(queryconfig, new RequestTableParameter(){
                    IsFilterByColumn = true,
                    OrderBy = null,
                    Query = JsonConvert.SerializeObject(new QueryFilter[] { new QueryFilter("FirstName", OperatorType.StartWith, "Car")}),
                    Page = 1,
                    PerPage = 10
                }).Result;

                System.IO.File.AppendAllText(@"C:\Users\hmota\Documents\MyProjects\PointOfSales\TablePlugin.Test\output.txt"
                , JsonConvert.SerializeObject(result));
                Assert.AreEqual(1,1);
            }); 
        }
    }


    public class DummyContext : DbContext
    {

        public DummyContext(DbContextOptions<DummyContext> options)
        : base(options)
        { }

        public DbSet<Person> Person { get; set; }
    }

    public class Person
    {
        public int PersonId {get;set;}
        public string FirstName {get; set;}
        public string LastName {get;set;}

        public int Age {get;set;}
    }
}
