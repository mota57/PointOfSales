using Newtonsoft.Json.Schema;
using System;
using PointOfSales.Core.DTO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj = new
            {
                username = "andrey",
                log = new JRaw("function() { return function(arg1) { console.log(arg1) }")
            };
            // and then serialize using the JsonConvert class
            var jsonObj = JsonConvert.SerializeObject(obj);
            Console.WriteLine(jsonObj);
            Console.ReadLine();
        }
    }
}
