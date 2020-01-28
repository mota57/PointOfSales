using SqlKata;

namespace TablePlugin.Core
{
    public interface IFilterByColumnStrategy
    {
        void FilterByColumn(Query query, QueryConfig queryConfig, IRequestParameterAdapter parameter);
    }
}
