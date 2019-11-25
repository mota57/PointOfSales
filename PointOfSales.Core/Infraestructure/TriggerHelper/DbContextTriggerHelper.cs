using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSales.Core.Infraestructure.TriggerHelper
{
    public class DbContextTriggerHelper : IBeforeCreate, IBeforeCreateAsync
    {
        public void BeforeCreate(DbContext context)
        {

            var addedEntries = GetEntries<IBeforeCreate>(context, EntityState.Added);
            foreach (var entry in addedEntries)
            {
                (entry.Entity as IBeforeCreate).BeforeCreate(context);
            }
        }

        public async Task BeforeCreateAsync(DbContext context)
        {
            var addedEntries = GetEntries<IBeforeCreateAsync>(context, EntityState.Added);
            foreach (var entry in addedEntries)
            {
                await (entry.Entity as IBeforeCreateAsync).BeforeCreateAsync(context);
            }
        }

        public IEnumerable<EntityEntry> GetEntries<IInterface>(DbContext context, EntityState entityState)
        {
            IEnumerable<EntityEntry> addedEntries = Enumerable.Empty<EntityEntry>();
            var entries = context.ChangeTracker.Entries()
               .Where(p => p.Entity is IInterface);
            if (entries != null)
                addedEntries = entries.Where(p => p.State == entityState);
            return addedEntries;

        }
    }


}
