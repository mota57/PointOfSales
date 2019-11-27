using Microsoft.EntityFrameworkCore;

namespace PointOfSales.Core.Infraestructure.TriggerHelper
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
