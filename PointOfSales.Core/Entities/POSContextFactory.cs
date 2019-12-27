using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PointOfSales.Core.Entities
{
    public class POSContextFactory : IDesignTimeDbContextFactory<POSContext>
    {
        public POSContext CreateDbContext(string[] args)
        {

            var helper = new PointOfSales.Core.Infraestructure.ConfigurationLoaderHelper();

            var optionsBuilder = new DbContextOptionsBuilder<POSContext>();
            optionsBuilder.UseSqlite(helper.Config["DBKEY"]);

            return new POSContext(optionsBuilder.Options);
        }

       
      
    }

}
