using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Scriban;
using Scriban.Runtime;
using PointOfSales.Core.Infraestructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PointOfSales.Core.Entities;

namespace PointOfSales.Core.Generator
{ 


    public abstract class AbstractBaseGenerator
    {
        public abstract Task Build(string inputFile, string outputFile, object objTemplate);
    }


    // We simply inherit from ScriptObject
    // All functions defined in the object will be imported
    public class GlobalScribanCustomFunctions : ScriptObject
    {
        public object Model { get; set; }


        public static string lowerinit(string word)
        {
            Console.WriteLine("lowerinit::word " + word);
            if(string.IsNullOrEmpty(word))
            {
                return "";
            }
            return  $"{word[0].ToString().ToLower()}{word.Substring(1, word.Length - 1)}";
        }
        //Example do not iomport script memeber
        //[ScriptMemberIgnore] // This method won't be imported
        //public static void NotImported()
        //{
        //    // ...
        //}
    }


    public class ScribanGeneratorConcrete : AbstractBaseGenerator
    {
        private string _templateDirectory { get; set; }
        private string _outputDirectory { get; set; }

        public ScribanGeneratorConcrete(string template = "", string outputDirectory = "")
        {

            var rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent;
            _templateDirectory = string.IsNullOrEmpty(template) ?  Path.Combine(rootDirectory.FullName, "Template") : template;
            _outputDirectory = string.IsNullOrEmpty(outputDirectory) ? Path.Combine(rootDirectory.FullName, "Output") : outputDirectory;
        }


        public override async Task Build(string inputFile, string outputFile, object objTemplate)
        {
            try
            {

                var templateContext = new TemplateContext();
                templateContext.MemberRenamer = member => member.Name;

                var globals = new GlobalScribanCustomFunctions();

                var scriptObjTemplate = new ScriptObject();
                scriptObjTemplate.Import(objTemplate, renamer:member => member.Name);

                templateContext.PushGlobal(globals);
                templateContext.PushGlobal(scriptObjTemplate);


                var path = Path.Combine(_templateDirectory, inputFile);
                var templateUncompile = await File.ReadAllTextAsync(path);

                var template = Template.Parse(templateUncompile);

                if (template.HasErrors == true)
                {
                    foreach (var error in template.Messages)
                    {
                        Console.WriteLine("error "+ error);
                    }
                    return;
                }

                var compiled = template.Render(templateContext);

                Console.WriteLine(compiled);

                await File.WriteAllTextAsync(Path.Combine(_outputDirectory, outputFile), compiled);

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }

    public static class ModelFormFactory
    {
        public static ModelForm BuildModelForm(IdentityDbContext<ApplicationUser> context, Type type)
        {
            var modelForm = new ModelForm();
            modelForm.ModelName = type.Name;
            var assembly = type.Assembly;
            var metadata = context.Entry(assembly.CreateInstance(type.FullName)).Metadata;
            //var foreignKeys = metadata.GetReferencingForeignKeys(); to get the collections for the entities where product is principal
            var properties = metadata.GetProperties().Where(_ => !_.IsShadowProperty);
            var formLayout = type.GetCustomAttribute<FormLayoutAttribute>();

            foreach (var item in  properties)
            {
                
                var fieldForm = new FieldForm()
                {
                    Label = item.Name,
                    Name = item.Name,
                    IsPrimaryKey = item.IsPrimaryKey()
                };
                modelForm.FieldForms.Add(fieldForm);

                if (item.IsForeignKey())
                {
                    IForeignKey foreignKey = item.GetContainingForeignKeys().First();
                    var declaringType = foreignKey.PrincipalKey.DeclaringEntityType;

                    fieldForm.ForeignEntityName = declaringType.ClrType.Name;
                    fieldForm.IsForeingKey = true;
                    fieldForm.IsCollection = foreignKey.DependentToPrincipal.IsCollection();
                    fieldForm.Type = "select";
                    modelForm.DependentToPrincipalFields.Add(fieldForm);
                } else
                {
                    fieldForm.Type = ModelFormHelper.GetTypeInput(item);
                }

                if(fieldForm.Type == "Upload")
                {
                    modelForm.ContainsFile = true;
                }

            }
            //order entities



            return modelForm;
        }
    }

    public class ModelFormHelper
    {

        
        /// <summary>
        /// get a type to render a proper input format
        /// </summary>
        /// <returns></returns>
        public static string GetTypeInput(IProperty item)
        {
            var attr = item.PropertyInfo.GetCustomAttribute<DataTypeAttribute>();
            if (attr != null)
            {

                Console.WriteLine($"attr.DataType {attr.DataType}");
                Console.WriteLine($"attr.CustomDataType {attr.CustomDataType}");
                return !string.IsNullOrEmpty(attr.CustomDataType) ? attr.CustomDataType : attr.DataType.ToString();
            }

            return item.ClrType.IsNumber() ? "number" :
                item.ClrType == typeof(DateTime) ? "date" :
                item.ClrType == typeof(string) ? "text" : throw new Exception($"not type assing to property {item.Relational().ColumnName}");
        }

    }

    public static class ExtensionHelper
    {
       
        public static bool IsNumber(this Type type)
        {
            return type == typeof(int) || type == typeof(decimal) || type == typeof(short) || type == typeof(double) || type == typeof(float) 
                || type == typeof(long) || type == (typeof(uint)) || type == typeof(ushort);
        }

    }

    public class ModelForm
    {
        public ModelForm()
        {
            FieldForms = new List<FieldForm>();
            DependentToPrincipalFields = new List<FieldForm>();
        }
        public string ModelName { get; set; }
        public bool ContainsFile { get; set; }
        public List<FieldForm> FieldForms { get; set; }
        public List<FieldForm> DependentToPrincipalFields { get; set; }
    }

    public class FieldForm
    {
        public bool IsPrimaryKey { get; internal set; }
        public string Label { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; } 
        public bool IsForeingKey { get; internal set; }
        public bool IsCollection { get; internal set; }
        /// <summary>
        /// Gets the entity type name for the foreign field
        /// </summary>
        public string ForeignEntityName { get; internal set; }
    }

}
