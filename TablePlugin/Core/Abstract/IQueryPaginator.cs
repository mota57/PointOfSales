using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TablePlugin.Core
{
    public interface IQueryPaginator<TResult>
    {
        QueryConfig QueryConfig { get; set; }
        Task<DataResponseAbstract<TResult>> GetAsync(IRequestParameterAdapter request);
    }
}
