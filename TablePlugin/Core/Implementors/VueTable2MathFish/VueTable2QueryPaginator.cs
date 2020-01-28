namespace TablePlugin.Core
{
    public class VueTable2QueryPaginator<TResult> : BaseQueryPaginatorAbstract<VueTable2Response<TResult>, TResult>
    {
        public VueTable2QueryPaginator(QueryConfig queryConfig) : base(queryConfig)
        {
        }
    }
}