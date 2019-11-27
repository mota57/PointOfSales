using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PointOfSales.Core.Infraestructure.TriggerHelper
{
    //public interface IBeforeCreateAsync
    //{
    //     Task BeforeCreateAsync(DbContext context);
    //}


    public interface IBeforeCreateAsync<TEntity>
    {
        Task BeforeCreateAsync(DbContext context, TEntity entity);
    }

}
