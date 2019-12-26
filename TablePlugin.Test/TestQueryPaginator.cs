using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TablePlugin.Core;
using TablePlugin.Data;

namespace TablePlugin.Test
{




    [TestClass]
    public class TestQueryPaginator {

        ILogger<TestQueryPaginator> Logger { get; }
        private string _connection =null;
        public string Connection { 
            get
            {
                if(_connection == null){
                    _connection = $"Data Source={UtilHelper.GetFilePath("dummy.db")}";
                }
                return _connection;

            }
        }

        QueryConfig QueryPersonInfo { get; }
        public TestQueryPaginator () {
            
            Logger = QueryTestHelper.GetLogger<TestQueryPaginator> ();
            
            
            TestHandler.Connection = Connection;

            QueryPersonInfo = new QueryConfig ("Person",
                        new QueryField ("FirstName"),
                        new QueryField ("LastName"),
                        new QueryField (name: "Age", type: "integer"));


            QueryPersonInfo.Provider = DatabaseProvider.SQLite;
            QueryPersonInfo.ConnectionString = Connection;
        }


        [TestMethod]
        public void TestFilterOperators () {
            TestHandler.Handle<DummyContext> ((options) => {

                using (var ctx = new DummyContext (options)) 
                {
                    ctx.Person.AddRange (
                        new Person () { FirstName = "Carlos", LastName = "Sank", Age = 23 },
                        new Person () { FirstName = "Mario", LastName = "Sank", Age = 40 },
                        new Person () { FirstName = "Carla", LastName = "Sank", Age = 11 },
                        new Person () { FirstName = "Juan", LastName = "Gabriel Montandon", Age = 40 }
                    );

                    ctx.SaveChanges();

                       //test startwith
                    var containsLetterA =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("FirstName", OperatorType.Contains, "A")) //ignoring the case
                                    .Result;


                    //test startwith
                    var startWithCAR =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("FirstName", OperatorType.StartWith, "Car"))
                                    .Result;

                    var montandonRecord =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("LastName", OperatorType.EndWith, "Montandon"))
                                    .Result;



                    var whereLastNameNotEqualSank = new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("LastName", OperatorType.NotEquals, "Sank"))
                                    .Result;

                    
                    var resultAllSank =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("LastName", OperatorType.Equals, "Sank"))
                                    .Result;


                    
                    var greaterThan11 =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("Age", OperatorType.GreaterThan, 11))
                                    .Result;


                    var over40 =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("Age", OperatorType.GreaterOrEqual, 40))
                                    .Result;

                    
                     var lessThan40 =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("Age", OperatorType.LessThan, 40))
                                    .Result;

                                    
                     var LessOrEqual40 =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterFilterBy ("Age", OperatorType.LessOrEqual, 40))
                                    .Result;

                    
                    //Assert.AreSame(1,0);

                    Assert.AreEqual(4, containsLetterA.Data.Count());    
                    
                    
                    Assert.AreEqual(1, montandonRecord.Data.Count());    
                    Assert.AreEqual("Gabriel Montandon", montandonRecord.Data.First().LastName);
                

                    Assert.AreEqual(1, whereLastNameNotEqualSank.Data.Count());

                    Assert.AreEqual(3, greaterThan11.Data.Count());
                    Assert.IsFalse(greaterThan11.Data.Any(x => x.FirstName.EqualIgnoreCase("Carla")));
                    

                    Assert.AreEqual(3, resultAllSank.Data.Count());

                    Assert.AreEqual(2, over40.Data.Count());
                    Assert.IsTrue(over40.Data.All(x => x.Age == 40));


                    
                    Assert.AreEqual(2, lessThan40.Data.Count());
                    Assert.IsTrue(lessThan40.Data.Any(x => x.FirstName.EqualIgnoreCase("Carlos")));
                    Assert.IsTrue(lessThan40.Data.Any(x => x.FirstName.EqualIgnoreCase("Carla")));
                    

                    Assert.AreEqual(4, LessOrEqual40.Data.Count());

                    Assert.AreEqual(2, startWithCAR.Data.Count());
                    Assert.IsTrue(startWithCAR.Data.All(p => p.FirstName.StartsWith("Car")));



                    ClearRecords(ctx);
                }
                
            });
           
        }



        
        [TestMethod]
        public void TestOrderBy () {
            TestHandler.Handle<DummyContext> ((options) => {

                using (var ctx = new DummyContext (options)) 
                {
                   
                    ctx.Person.AddRange (
                        new Person () { FirstName = "Carlos", LastName = "Sank", Age = 23 },
                        new Person () { FirstName = "Mario", LastName = "Sank", Age = 40 },
                        new Person () { FirstName = "Carla", LastName = "Sank", Age = 11 },
                        new Person () { FirstName = "Juan", LastName = "Gabriel Montandon", Age = 40 }
                       );

                    ctx.SaveChanges ();

                    var resultForNames =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterOrderBy ("FirstName", OrderType.ASC))
                                    .Result;
                    
                    var orderByFirstName = resultForNames.Data.Select(x => x.FirstName).ToArray();
                                    
                    var resultForAge =  new QueryPaginatorBasic()
                                    .GetAsync<Person> (QueryPersonInfo,
                                     BuildRequestParameterOrderBy ("Age", OrderType.ASC))
                                    .Result;

                    var orderByAge = resultForAge.Data.Select(x => x.Age).ToArray();
                                    

                                         
                    QueryTestHelper.LogToFile<TestQueryPaginator>(orderByAge);

                    Assert.AreEqual("Carla", orderByFirstName[0]);
                    Assert.AreEqual("Carlos", orderByFirstName[1]);
                    Assert.AreEqual("Juan", orderByFirstName[2]);
                    Assert.AreEqual("Mario", orderByFirstName[3]);


                    Assert.AreEqual(11, orderByAge[0]);
                    Assert.AreEqual(23, orderByAge[1]);
                    Assert.AreEqual(40, orderByAge[2]);
                    Assert.AreEqual(40, orderByAge[3]);

                    ClearRecords(ctx);
                    
                }

            });
           
        }


          
        [TestMethod]
        public void TestPagination () {
            TestHandler.Handle<DummyContext> ((options) => {

                using (var ctx = new DummyContext (options)) 
                {  
                    ctx.Person.AddRange (
                        new Person () { FirstName = "Carlos", LastName = "Sank", Age = 23 },
                        new Person () { FirstName = "Mario", LastName = "Sank", Age = 40 },
                        new Person () { FirstName = "Carla", LastName = "Sank", Age = 11 },
                        new Person () { FirstName = "Juan", LastName = "Gabriel Montandon", Age = 40 }
                       );

                    ctx.SaveChanges ();

                    var parameters = new RequestTableParameter () {
                        IsFilterByColumn = true,
                        OrderBy = null,
                        Query =  null,
                        Page = 1,
                        PerPage = 2,
                    };
               
                    var firstPage =  new QueryPaginatorBasic()
                              .GetAsync<Person> (QueryPersonInfo,parameters).Result;

                    
                    Assert.AreEqual(2, firstPage.Data.Count());

                    parameters.Page = 2;
                    var secondPage =  new QueryPaginatorBasic()
                              .GetAsync<Person> (QueryPersonInfo,parameters).Result;


                             
                    Assert.AreEqual(2, firstPage.Data.Count());

                    ClearRecords(ctx);
                    
                }
            });
           
        }





        private void ClearRecords(DummyContext ctx){
            if(ctx.Person.Any())
            {
                ctx.RemoveRange (ctx.Person.ToList ());
                ctx.SaveChanges ();
            }
        }
        
        private RequestTableParameter BuildRequestParameterFilterBy (string name, OperatorType @operator, object value, int page = 1) {
            return new RequestTableParameter () {
                IsFilterByColumn = true,
                    OrderBy = null,
                    Query = JsonConvert.SerializeObject (new QueryFilter[] { new QueryFilter (name, @operator, value) }),
                    Page = page,
                    PerPage = 10,
            };
        }


           private RequestTableParameter BuildRequestParameterOrderBy (string name, OrderType orderType, int page = 1) {
            return new RequestTableParameter () {
                IsFilterByColumn = true,
                    OrderBy = new PropertyOrder[] { new PropertyOrder(name, orderType)} ,
                    Query =  null,
                    Page = page,
                    PerPage = 10,
            };
        }
    }

    public class DummyContext : DbContext {

        public DummyContext (DbContextOptions<DummyContext> options) : base (options) { 

        }
        

        public DbSet<Person> Person { get; set; }
    }

    public class Ticket {
        public int TicketId {get; set;}
        public decimal Price {get;set;}
    }

    public class Person {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Age { get; set; }
    }
}