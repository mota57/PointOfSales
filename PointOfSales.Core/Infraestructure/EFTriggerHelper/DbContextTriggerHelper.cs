using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PointOfSales.Core.Infraestructure.EFTriggerHelper
{
    public static class TriggerHelper
    {
        private static List<Type> TypeImplementationOfBeforeCreate = new List<Type>();
        private static HashSet<string> triggerToByPass = new HashSet<string>();
        public static void ByPass(string name)
        {
            triggerToByPass.Add(name);
        }

        public static bool IsTriggerInByPassList(string name) => triggerToByPass.Remove(name);

    }

    //public static class StoreTriggerInit
    //{
    //    public static void FindImplementations()
    //    {

    //    }
    //}

    public class DbContextTriggerHelper
    {
        private Assembly assembly = null;

        public DbContextTriggerHelper(Assembly assembly = null)
        {
            this.assembly = assembly;
        }


        public void BeforeCreate(DbContext context)
        {
            //var addedEntries = GetEntries<IBeforeCreate>(context, EntityState.Added);
            //foreach (var entry in addedEntries)
            //{
            //    (entry.Entity as IBeforeCreate).BeforeCreate(context);
            //}
            if (assembly != null)
            {
                var typeList = GetTypesWithInferfaceOfType(assembly, typeof(IBeforeCreate<>));
                var methodName = nameof(BeforeCreate);
                ExecuteTriggerMethod(context, typeList, methodName, EntityState.Added);
            }
        }

        public void ExecuteTriggerMethod(DbContext context, IEnumerable<MetaGenericType> typeList, string methodName, EntityState entityState)
        {
            foreach (var typeMeta in typeList)
            {
                var type = typeMeta.Implementor;
                var entries = GetEntries(context, typeMeta.EntityTypeArg, entityState);

                object instance = assembly.CreateInstance(type.FullName, false,
                     BindingFlags.ExactBinding,
                     null, new object[] { }, null, null);

                MethodInfo info = assembly.GetType(type.FullName).GetMethod(methodName);

                foreach (var entry in entries)
                    info.Invoke(instance, new object[] { context, entry.Entity });
            }
        }

        public class MetaGenericType
        {
            public Type Implementor { get; set; }
            public Type EntityTypeArg { get; set; }
        }

        public IEnumerable<MetaGenericType> GetTypesWithInferfaceOfType(Assembly assembly, Type typeInterface)
        {
            List<MetaGenericType> typeMatch = new List<MetaGenericType>();
            var types  = assembly.GetTypes();
            
            foreach(var t in types)
            {
                foreach(var intf in t.GetInterfaces())
                {
                    if(intf.IsGenericType && intf.GetGenericTypeDefinition() == typeInterface)
                    {
                        typeMatch.Add(new MetaGenericType()
                        {
                            EntityTypeArg = intf.GetGenericArguments().First(),
                            Implementor = t
                        });
                    }
                }
            }
            
              //var result = types.Where(type =>
              //  type.GetInterfaces().Any(intf => intf.GetGenericTypeDefinition() == typeInterface)
              //  //&& !TriggerHelper.IsTriggerInByPassList(type.Name) 
              //);
            return typeMatch;
        }

      
        public IEnumerable<EntityEntry> GetEntries(DbContext context, Type type, EntityState entityState)
        {
            return context.ChangeTracker.Entries()
               .Where(p => p.Entity.GetType() == type
                && p.State == entityState);
        }


        //public async Task BeforeCreateAsync(DbContext context)
        //{
        //var addedEntries = GetEntries<IBeforeCreateAsync>(context, EntityState.Added);
        //foreach (var entry in addedEntries)
        //{
        //    await (entry.Entity as IBeforeCreateAsync).BeforeCreateAsync(context);
        //}
        //}
    }


}
