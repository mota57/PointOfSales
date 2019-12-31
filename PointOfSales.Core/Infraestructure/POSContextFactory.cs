using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PointOfSales.Core.Entities;

namespace PointOfSales.Core
{
    public class POSContextFactory : IDesignTimeDbContextFactory<POSContext>
    {
        public POSContext CreateDbContext(string[] args)
        {

            var helper = new Infraestructure.ConfigurationLoaderHelper();
            var optionsBuilder = new DbContextOptionsBuilder<POSContext>();
            optionsBuilder.UseSqlite(helper.GetConnectionString());
            return new POSContext(optionsBuilder.Options);
        }

       
      
    }

}
