using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PointOfSales.Core.Entities
{
    public class POSContextFactory : IDesignTimeDbContextFactory<POSContext>
    {
        public POSContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<POSContext>();
            optionsBuilder.UseSqlite(GlobalVariables.Connection);

            return new POSContext(optionsBuilder.Options);
        }
    }

    

}
