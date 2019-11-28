using Microsoft.EntityFrameworkCore;

namespace PointOfSales.Core.Infraestructure.EFTriggerHelper
{
    //public interface IBeforeUpdate
    //{
    //    void BeforeUpdate(DbContext context);
    //}


    public interface IBeforeUpdate<TEntity>
    {
        void BeforeUpdate(DbContext context, TEntity entity);
    }
}
