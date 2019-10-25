using Newtonsoft.Json.Schema;
using System;
using System.Linq;
using PointOfSales.Core.DTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PointOfSales.Core.Infraestructure.VueTable;
using SqlKata.Execution;
using PointOfSales.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using Scriban;
using PointOfSales.Core.Generator;

namespace ConsoleApp1
{
    public class Program
    {

        static Action<string> Log = (msg) => Console.WriteLine(msg);

        static void Main(string[] args)
        {

            //example_log_modelFactory(); //log modelform example
            ProgramGenerator.example_render_model(); //render app

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

        public static void RunVueReader()
        {

            VueTableReader reader = new VueTableReader();
            var db = reader.BuildQueryFactory();


            var q = db.Query("Modifier")
                           .Select("Modifier.Id", "Modifier.Name")
                           .SelectRaw("(Select (COUNT(Name) || 'Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount");
                            
               var result =         q.Get();

            Console.WriteLine(JsonConvert.SerializeObject(result));

        }
    }
}
