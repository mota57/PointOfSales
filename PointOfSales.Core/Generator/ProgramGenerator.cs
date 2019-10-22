using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointOfSales.Core.Generator
{
    public static class ProgramGenerator
    {
        static Action<string> Log = (msg) => Console.WriteLine(msg);

        public static void example_render_model()
        {
            POSContext context = new POSContextFactory().CreateDbContext(new string[] { });
            ModelForm model = ModelFormFactory.BuildModelForm(context, typeof(Order));
            AbstractBaseGenerator generator = new ScribanGeneratorConcrete();
            generator.Build("form-entity.scriban", "form-order.vue", model).Wait();
            Log("complete");
        }

        public static void example_log_modelFactory()
        {

            POSContext context = new POSContextFactory().CreateDbContext(new string[]{ });
            ModelForm model = ModelFormFactory.BuildModelForm(context, typeof(Product));
            Log(JsonConvert.SerializeObject(model, Formatting.Indented));
        }

        public static void example_log_properties()
        {

            string separator = "===========================================";
            POSContext context = new POSContextFactory().CreateDbContext(new string[]{ });
            var metadata = context.Entry(new Product()).Metadata;
            var props = metadata.GetProperties();
            var navigations = metadata.GetNavigations();

            foreach(var nav in navigations)
            {

                Log("\n\n");

                Log("naviagtions.....");

                Log($"nav.Name : {nav.Name}");
                Log($"nav.GetFieldName(): {nav.GetFieldName()}");
                Log($"nav.IsCollection(): {nav.IsCollection()}");
                Log($"nav.IsDependentToPrincipal(): {nav.IsDependentToPrincipal()}");
                Log($"nav.ClrType.Name: {nav.ClrType.Name}");
                Log($"nav.DeclaringType.Name: {nav.DeclaringType.Name}");
                Log($"nav.DeclaringEntityType.Name: {nav.DeclaringEntityType.Name}");

                Log("\n\n");

            }

            foreach (var item in props)
            {
                if(item.Name == "CategoryId")
                {
                    Log("*********************************");
                    var foreignKey = item.GetContainingForeignKeys().First();



                    Log($"PrincipalToDependent {foreignKey.PrincipalToDependent.Name}");
                    Log($"DependentToPrincipal {foreignKey.DependentToPrincipal.Name}");//category
                    Log($"DeclaringEntityType {foreignKey.DeclaringEntityType.Name}");//product
                    Log($"PrincipalKey Name  {foreignKey.PrincipalKey.Relational().Name}"); //category
                    if(item.IsForeignKey() && foreignKey.DependentToPrincipal.IsCollection() == false)
                    {

                        Log("*********************************");

                        Log($"foreignKey Relational {foreignKey.Relational().Name}");
                        var declaringType = foreignKey.PrincipalKey.DeclaringEntityType;
                        var meta = declaringType.Relational();
                        Log($"declaringType.ClrType.Name {declaringType.ClrType.Name}");
                        Log($"\tdeclaringType..Name {declaringType.Name}");
                        Log($"\tdeclaringType.Model.ToString() {declaringType.Model.ToString()}");
                        Log($"\tdeclaringType.Relational().TableName {meta.TableName}");
                        Log($"\tdeclaringType.Relational().Schema {meta.Schema}");
                        Log($"\tdeclaringType.Relational().DiscriminatorValue {meta.DiscriminatorValue}");
                    }

                    Log(separator);

                    Log("*********************************");
                }
                Console.WriteLine($"Name:{item.Name}, type : {item.ClrType}");

            }

            Console.WriteLine("=============================");
            var ikeys = metadata.GetForeignKeys();
            foreach (var item in ikeys)
            {
                Console.WriteLine($"ToString:{item.PrincipalKey.ToString()}");

            }
            

        }

 
    }
}
