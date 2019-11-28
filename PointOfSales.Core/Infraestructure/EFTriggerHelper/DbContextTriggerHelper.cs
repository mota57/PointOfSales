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
                ExecuteTriggerMethod(context, typeList, methodName);
            }
        }

        public void ExecuteTriggerMethod(DbContext context, IEnumerable<Type> typeList, string methodName)
        {
            foreach (var type in typeList)
            {
                var typeArg = type.GetGenericArguments().First();
                var entries = context.ChangeTracker.Entries().Where(entry => entry.GetType() == typeArg);

                object instance = assembly.CreateInstance(type.Name, false,
                     BindingFlags.ExactBinding,
                     null, new object[] { }, null, null);

                MethodInfo info = assembly.GetType(type.FullName).GetMethod(methodName);

                foreach (var entry in entries)
                    info.Invoke(instance, new object[] { context, entry });
            }
        }

        public IEnumerable<Type> GetTypesWithInferfaceOfType(Assembly assembly, Type typeInterface)
        {
            return assembly.GetTypes()
              .Where(type =>
                type.GetInterfaces().Any(intf => intf.GetGenericTypeDefinition() == typeInterface)
                && !TriggerHelper.IsTriggerInByPassList(type.Name) 
              );
        }

      
        //public async Task BeforeCreateAsync(DbContext context)
        //{
            //var addedEntries = GetEntries<IBeforeCreateAsync>(context, EntityState.Added);
            //foreach (var entry in addedEntries)
            //{
            //    await (entry.Entity as IBeforeCreateAsync).BeforeCreateAsync(context);
            //}
        //}


        public IEnumerable<EntityEntry> GetEntries(Type type, DbContext context, EntityState entityState)
        {
            IEnumerable<EntityEntry> addedEntries = Enumerable.Empty<EntityEntry>();
            var entries = context.ChangeTracker.Entries()
               .Where(p => p.Entity.GetType() == type);
            if (entries != null)
                addedEntries = entries.Where(p => p.State == entityState);
            return addedEntries;

        }
    }


}
