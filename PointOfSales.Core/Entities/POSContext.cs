using EFTriggerHelper;
using EFTriggerHelper.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PointOfSales.Infraestructure.TriggerClasses;
using System;
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
                new Category() { Id=1, Name = "Category 1", CreateDate = DateTime.Now, ModifiedDate = DateTime.Now}
                ,new Category() { Id =2, Name = "Category 2", CreateDate = DateTime.Now, ModifiedDate = DateTime.Now }
                ,new Category() { Id=3, Name = "Category 3", CreateDate = DateTime.Now, ModifiedDate = DateTime.Now }
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

        private DbContextTriggerHelper helper = new DbContextTriggerHelper(typeof(OrderTrigger).Assembly);

        public override int SaveChanges()
        {
            Func<int> handler = () => base.SaveChanges();
            return this.EFTriggerHelperSaveChanges(handler, helper);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            Func<int> handler = () => base.SaveChanges(acceptAllChangesOnSuccess);
            return this.EFTriggerHelperSaveChanges(handler, helper);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            Func<Task<int>> handler = async () => await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            return await this.EFTriggerHelperSaveChangesAsync(handler, helper);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Func<Task<int>> handler = async () => await base.SaveChangesAsync(cancellationToken);
            return await this.EFTriggerHelperSaveChangesAsync(handler, helper);
        }
    }


}
