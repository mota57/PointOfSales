
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PointOfSales.Core.Infraestructure.VueTable;
using SqlKata.Execution;

using PointOfSales.Core.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TablePlugin;

namespace ConsoleApp1
{
    public class Program
    {

        static Action<string> Log = (msg) => Console.WriteLine(msg);

        static void Main(string[] args)
        {

            //example_log_modelFactory(); //log modelform example
            //ProgramGenerator.example_render_model(); //render app
            try
            {

                Example_Pagination();

            } catch (Exception ex)
            {
                Console.WriteLine($"Application error {ex.Message}");
                Console.WriteLine($"Application stacktrace {ex.StackTrace}");
            }
            Console.ReadLine();
        }

        //public static void example_run_example_json()
        //{

        //    var obj = new
        //    {
        //        username = "andrey",
        //        log = new JRaw("'function() { return function(arg1) { console.log(arg1) }'")
        //    };
        //    Log(JsonConvert.SerializeObject(obj));

        //}

        public static void RunVueReader()
        {

            VueTableReader reader = new VueTableReader();
            var db = PointOfSales.Core.Infraestructure.VueTable.QueryFactoryBuilder.BuildQueryFactory();


            var q = db.Query("Modifier")
                           .Select("Modifier.Id", "Modifier.Name")
                           .SelectRaw("(Select (COUNT(Name) || 'Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount");

            var result = q.Get();

            Console.WriteLine(JsonConvert.SerializeObject(result));

        }

        public static void Example_Pagination()
        {
            var queryConfig = new CustomQueryConfig("v_tracks",
                    new QueryField("TrackId"),
                    new QueryField("Name"),
                    new QueryField("album"),
                    new QueryField("media"),
                    new QueryField("geners"));

            queryConfig.ConnectionString = @"Data Source=C:\Users\hmota\Documents\RESOURCES\Projects\PointOfSales\PointOfSales.AppConsole\chinook.db";

            queryConfig.Provider = TablePlugin.DatabaseProvider.SQLite;

            var queryParameter = new RequestTableParameter() { Page = 1, Query = null };

            Func<string, string> ToJObjectString = (string input) => {
                if (! string.IsNullOrEmpty(input))
                {
                    JObject obj = new JObject();
                    foreach (var item in input.Split(";"))
                    {
                        var keyValue = item.Split("=");
                        obj[keyValue[0]] = keyValue[1];
                    }
                    string result = JsonConvert.SerializeObject(obj);
                    Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
                    return result;
                }
                return null;
            };

            Func<string, PropertyOrder[]> ToPropertyOrder = (string input) =>
            {

                ICollection<PropertyOrder> propertyOrders = new List<PropertyOrder>();
            if (!string.IsNullOrEmpty(input))
               {
                   JObject jobj =  JsonConvert.DeserializeObject<JObject>(input);
                   foreach(var jprop in jobj.Properties())
                   {
                       if (!jprop.HasValues) continue;
                       OrderType type = (OrderType)Enum.Parse(typeof(OrderType), jprop.Value.ToString());
                       propertyOrders.Add(new PropertyOrder(jprop.Name, type));
                   }
               };
               return propertyOrders.ToArray();
           };
            
            while (true)
            {

                Console.WriteLine("enter page");
                queryParameter.Page = int.Parse(Console.ReadLine());

                Console.WriteLine("enter where clause");
                queryParameter.Query = ToJObjectString(Console.ReadLine());
             
                Console.WriteLine("enter order by");
                queryParameter.OrderBy = ToPropertyOrder(ToJObjectString(Console.ReadLine()));

                Task.Run(async () =>
                {
                    CustomQueryWithPagination reader = new CustomQueryWithPagination();
                    var records = await reader.GetAsync(queryConfig, queryParameter);
                    Console.WriteLine(JsonConvert.SerializeObject(records, Formatting.Indented));
                    Console.WriteLine("=======================================================================\n\n");
                }).Wait();
            }

        }
    }
}
