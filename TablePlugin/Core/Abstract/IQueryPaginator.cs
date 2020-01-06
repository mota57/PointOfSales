using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TablePlugin.Core
{
    public interface IQueryPaginator
    {
        QueryConfig QueryConfig { get; set; }
        Task<object> GetAsync(IRequestParameter request);
        Task<DataResponse<TData>> GetAsync<TData>(IRequestParameter request);
    }
}
