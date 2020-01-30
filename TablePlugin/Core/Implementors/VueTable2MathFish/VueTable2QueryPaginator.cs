namespace TablePlugin.Core
{
    public class VueTable2QueryPaginator<TResult> : QueryPaginator<VueTable2Response<TResult>, TResult>
    {
        public VueTable2QueryPaginator(QueryConfig queryConfig) : base(queryConfig)
        {
        }
    }
}