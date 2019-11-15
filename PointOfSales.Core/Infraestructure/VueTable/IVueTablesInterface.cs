using System.Collections.Generic;
using System.Threading.Tasks;

namespace PointOfSales.Core.Infraestructure.VueTable
{
    public interface IVueTablesInterface
    {
        Task<Dictionary<string, object>> GetAsync(VueTableConfig config, VueTableParameters parameters);
    }
}
