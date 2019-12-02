using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PointOfSales.Core.Infraestructure.EFTriggerHelper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PointOfSales.Core.Entities
{

    public class ApplicationUser : IdentityUser
    {
        public string CustomTag { get; set; }
    }


    public class POSContext : IdentityDbContext<ApplicationUser>
    {

        public POSContext(DbContextOptions<POSContext> options)
        : base(options)
        { }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Modifier> Modifier { get; set; }
        public DbSet<ItemModifier> ItemModifier { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        //public DbSet<ProductModifier> ItemModifier { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id=1, Name = "Category 1"}
                ,new Category() { Id =2, Name = "Category 2"}
                ,new Category() { Id=3, Name = "Category 3"}
                );

            #region productModifier

            modelBuilder.Entity<ProductModifier>()
                .HasKey(_ => new { _.ModifierId, _.ProductId });

            modelBuilder.Entity<ProductModifier>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.ProductModifier)
                .HasForeignKey(pm => pm.ProductId);

            modelBuilder.Entity<ProductModifier>()
                .HasOne(pm => pm.Modifier)
                .WithMany(m => m.ProductModifier)
                .HasForeignKey(pm => pm.ModifierId);
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        //DbContextTriggerHelper helper = new DbContextTriggerHelper((typeof().Assembly);

        public override int SaveChanges()
        {
            //helper.BeforeCreate(this);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //helper.BeforeCreate(this);
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
