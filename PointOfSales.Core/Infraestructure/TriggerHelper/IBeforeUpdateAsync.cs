using Microsoft.EntityFrameworkCore;

namespace PointOfSales.Core.Infraestructure.TriggerHelper
{
    public interface IBeforeUpdateAsync
    {
        void BeforeUpdateAsync(DbContext context);
    }


}
