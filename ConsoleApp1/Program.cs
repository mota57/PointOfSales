using Newtonsoft.Json.Schema;
using NJsonSchema.Generation;
using PointOfSales.Core.Entities;
using System;
using JsonSchemaGenerator = NJsonSchema.Generation.JsonSchemaGenerator;
using JsonSchemaResolver = NJsonSchema.Generation.JsonSchemaResolver;
using JsonSchema = NJsonSchema.JsonSchema;
using PointOfSales.Core.DTO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonSchemaGeneratorSettings settings = new JsonSchemaGeneratorSettings();
            var schema = JsonSchema.FromType<ProductDTO>();
            var schemaData = schema.ToJson();
            Console.WriteLine(schemaData);
            Console.ReadLine();
        }
    }
}
