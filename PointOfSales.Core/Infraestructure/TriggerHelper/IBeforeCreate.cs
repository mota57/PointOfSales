using Microsoft.EntityFrameworkCore;

namespace PointOfSales.Core.Infraestructure.TriggerHelper
{
    public interface IBeforeCreate
    {
        void BeforeCreate(DbContext context);
    }


}
