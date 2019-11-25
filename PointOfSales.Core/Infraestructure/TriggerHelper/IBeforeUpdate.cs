using Microsoft.EntityFrameworkCore;

namespace PointOfSales.Core.Infraestructure.TriggerHelper
{
    public interface IBeforeUpdate
    {
        void BeforeUpdate(DbContext context);
    }


}
