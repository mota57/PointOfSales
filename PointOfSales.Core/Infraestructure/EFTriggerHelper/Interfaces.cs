using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PointOfSales.Core.Infraestructure.EFTriggerHelper
{

    public interface IBeforeCreate<TEntity> 
    {
        void BeforeCreate(DbContext context, TEntity entity);
    }

    public interface IAfterCreate<TEntity> 
    {
        void AfterCreate(DbContext context, TEntity entity);
    }


    public interface IBeforeCreateAsync<TEntity>
    {
        Task BeforeCreateAsync(DbContext context, TEntity entity);
    }


    public interface IBeforeUpdate<TEntity>
    {
        void BeforeUpdate(DbContext context, TEntity entity);
    }

    public interface IBeforeUpdateAsync<TEntity>
    {
        void BeforeUpdateAsync(DbContext context, TEntity entity);
    }


}

