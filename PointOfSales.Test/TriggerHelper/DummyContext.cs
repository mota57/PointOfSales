using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Infraestructure.EFTriggerHelper;

namespace PointOfSales.Test.TriggerHelper
{
    public class DummyContext : DbContext
    {
        public DbSet<PersonTbl> Person { get; set; }
        public DbSet<LogTbl> LogTbl { get; set; }

        public DummyContext(DbContextOptions<DummyContext> options)
            : base(options)
        {
            helper = new DbContextTriggerHelper(typeof(PersonTrigger).Assembly);
        }


        DbContextTriggerHelper helper { get; set; }

        public override int SaveChanges()
        {
            helper.BeforeCreate(this);
            helper.BeforeUpdate(this);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            helper.BeforeCreate(this);
            helper.BeforeUpdate(this);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        //public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    await helper.BeforeCreateAsync(this);
        //    return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    await helper.BeforeCreateAsync(this);
        //    return await base.SaveChangesAsync(cancellationToken);
        //}

    }
}
