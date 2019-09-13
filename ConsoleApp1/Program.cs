using Newtonsoft.Json.Schema;
using System;
using PointOfSales.Core.DTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PointOfSales.Core.Infraestructure.VueTable;
using SqlKata.Execution;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var obj = new
            //{
            //    username = "andrey",
            //    log = new JRaw("function() { return function(arg1) { console.log(arg1) }")
            //};
            // and then serialize using the JsonConvert class
            //var jsonObj = JsonConvert.SerializeObject(obj);

            VueTableReader reader = new VueTableReader();
            var db = reader.BuildQueryFactory();


            var q = db.Query("Modifier")
                           .Select("Modifier.Id", "Modifier.Name")
                           .SelectRaw("(Select (COUNT(Name) || 'Modifiers') from ItemModifier WHERE  ItemModifier.ModifierId = Modifier.Id) as ModifierCount");
                            
               var result =         q.Get();

            Console.WriteLine(JsonConvert.SerializeObject(result));
            Console.ReadLine();
        }
    }
}
