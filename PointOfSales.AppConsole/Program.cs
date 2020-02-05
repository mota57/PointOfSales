
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PointOfSales.Core.Infraestructure.VueTable;
using SqlKata.Execution;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TablePlugin;
using TablePlugin.Core;
using TablePlugin.Data;
using PointOfSales.Core.Infraestructure;
using PointOfSales.Core.Generator;

namespace ConsoleApp1
{
    public class Program
    {

        static Action<string> Log = (msg) => Console.WriteLine(msg);

        static void Main(string[] args)
        {

            try
            {
                ProgramGenerator.example_render_model(); //render app

                //example_log_modelFactory<Product>(); //log modelform example
                // Example_Pagination();
                //RunConfiguration();

            } catch (Exception ex)
            {
                Console.WriteLine($"Application error {ex.Message}");
                Console.WriteLine($"Application stacktrace {ex.StackTrace}");
            }
            Console.ReadLine();
        }

        public static void example_run_example_json()
        {

            var obj = new
            {
                username = "andrey",
                log = new JRaw("'function() { return function(arg1) { console.log(arg1) }'")
            };
            Log(JsonConvert.SerializeObject(obj));

        }
        public static void RunConfiguration(){
            ConfigurationLoaderHelper loader = new ConfigurationLoaderHelper();
      
            while(true){
                Console.WriteLine("hey made some changes on th .config ");
                Console.ReadLine();
                Console.WriteLine(loader.Config.GetSection("DatabaseConfig")["DBKEY"].ToString());
            }

        }


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
    }
}
