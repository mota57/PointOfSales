using Microsoft.EntityFrameworkCore;

namespace PointOfSales.Core.Infraestructure.EFTriggerHelper
{
    //public interface IBeforeCreate
    //{
    //    void BeforeCreate(DbContext context);
    //}


    public interface IBeforeCreate<TEntity> 
    {
        void BeforeCreate(DbContext context, TEntity entity);
    }

}
