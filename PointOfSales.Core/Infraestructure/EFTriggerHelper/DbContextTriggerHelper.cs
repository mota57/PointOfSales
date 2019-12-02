using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PointOfSales.Core.Infraestructure.EFTriggerHelper
{
    //public static class TriggerGlobals
    //{
    //    private static List<Type> TypeImplementationOfBeforeCreate = new List<Type>();
    //    //private static HashSet<string> triggerToByPass = new HashSet<string>();
    //    //public static void ByPass(string name)
    //    //{
    //    //    triggerToByPass.Add(name);
    //    //}
    //    //public static bool IsTriggerInByPassList(string name) => triggerToByPass.Remove(name);

    //}


    public class MetaGenericType
    {
        public Type Implementor { get; set; }
        public Type EntityTypeArg { get; set; }
    }

    public class DbContextTriggerHelper
    {
        private Assembly assembly;

        public DbContextTriggerHelper(Assembly assembly)
        {
            this.assembly = assembly;
        }


        public void BeforeCreate(DbContext context)
        {
            var typeList = GetTypesWithInferfaceOfType(typeof(IBeforeCreate<>));
            var methodName = nameof(BeforeCreate);
            ExecuteTriggerMethod(context, typeList, methodName, EntityState.Added);
        }



        public void AfterCreate(DbContext context)
        {
            //var typeList = GetTypesWithInferfaceOfType(typeof(IAfterCreate<>));
            //var methodName = nameof(AfterCreate);
            //ExecuteTriggerMethod(context, typeList, methodName, EntityState.);
        }

        public void BeforeUpdate(DbContext context)
        {
            var typeList = GetTypesWithInferfaceOfType(typeof(IBeforeUpdate<>));
            var methodName = nameof(BeforeUpdate);
            ExecuteTriggerMethod(context, typeList, methodName, EntityState.Modified);
        }

        public void ExecuteTriggerMethod(DbContext context, IEnumerable<MetaGenericType> typeList, string methodName, EntityState entityState)
        {
            foreach (var typeMeta in typeList)
            {
                var type = typeMeta.Implementor;
                var entries = GetEntries(context, typeMeta.EntityTypeArg, entityState);
                if (entries.Count() == 0) continue;

                object instance = assembly.CreateInstance(type.FullName, false,
                     BindingFlags.ExactBinding,
                     null, new object[] { }, null, null);

                MethodInfo info = assembly.GetType(type.FullName).GetMethod(methodName);

                foreach (var entry in entries)
                    info.Invoke(instance, new object[] { context, entry.Entity });
            }
        }

        

        public IEnumerable<MetaGenericType> GetTypesWithInferfaceOfType(Type typeInterface)
        {
            List<MetaGenericType> typeListMatch = new List<MetaGenericType>();
            var types  = assembly.GetTypes();
            
            foreach(var type in types)
            {
                foreach(var intf in type.GetInterfaces())
                {
                    if(intf.IsGenericType && intf.GetGenericTypeDefinition() == typeInterface)
                    {

                        typeListMatch.Add(new MetaGenericType()
                        {
                            EntityTypeArg = intf.GetGenericArguments().First(),
                            Implementor = type
                        });
                    }
                }
            }
            
            return typeListMatch;
        }

      
        public IEnumerable<EntityEntry> GetEntries(DbContext context, Type type, EntityState entityState)
        {
            return context.ChangeTracker.Entries()
               .Where(p => p.Entity.GetType() == type
                && p.State == entityState);
        }
    }


}
