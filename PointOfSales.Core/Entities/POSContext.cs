using Microsoft.EntityFrameworkCore;

namespace PointOfSales.Core.Entities
{
    public class POSContext : DbContext 
    {

        public POSContext(DbContextOptions<POSContext> options)
        : base(options)
        { }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id=1, Name = "Category 1"}
                ,new Category() { Id =2, Name = "Category 2"}
                ,new Category() { Id=3, Name = "Category 3"}
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(GlobalVariables.Connection);
            }
        }
    }


}
