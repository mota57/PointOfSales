using SqlKata.Execution;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TablePlugin.Core
{
    public class QueryPaginator<DataResponseType, TResult> : IQueryPaginator<TResult>
         where DataResponseType : DataResponseAbstract<TResult>

    {

        public QueryPaginator(QueryConfig queryConfig)
        {
            this.QueryConfig = queryConfig;
            this.filterByColumnStrategy = new BasicFilterByColumnStrategy();
        }



        public QueryPaginator(QueryConfig queryConfig, IFilterByColumnStrategy filterByColumnStrategy)
        {
            this.QueryConfig = queryConfig;
            this.filterByColumnStrategy = filterByColumnStrategy;
        }


      

        private const int DEFAULT_PER_PAGE = 10;
        private int _perPage = DEFAULT_PER_PAGE;

        private IFilterByColumnStrategy filterByColumnStrategy =  new BasicFilterByColumnStrategy();

        public IFilterByColumnStrategy FilterByColumnStrategy
        {
            get => filterByColumnStrategy;
            set
            {
                filterByColumnStrategy = value ?? throw new ApplicationException("Column Strategy is required");
            }
        }

        public int PerPage
        {
            get { return _perPage; }
            set
            {
                if (value < 0)
                    _perPage = DEFAULT_PER_PAGE;
                else
                    _perPage = value;
            }
        }

        public virtual QueryConfig QueryConfig { get; set; }

        protected SqlKata.Query QueryBuilder { get; set; }


        public async Task<DataResponseAbstract<TResult>> GetAsync(IRequestParameterAdapter request)
        {
            BuildFilterOrderAndPaginateQuery(request);
            var dataResponseInstance = await QueryData();
            return CreateResponseAdapter(dataResponseInstance);
        }


        private void BuildFilterOrderAndPaginateQuery(IRequestParameterAdapter parameter)
        {
            QueryBuilder = QueryConfig.Query.Clone();
            PerPage = parameter.PerPage;

            Filter(parameter);
            OrderBy(parameter);
            Paginate(parameter);

        }


        protected virtual void OrderBy(IRequestParameterAdapter parameter)
        {
            var propertiesOrder = parameter.OrderBy;

            if (propertiesOrder == null) return;

            foreach (var item in propertiesOrder)
            {
                var isNotSorteable = !QueryConfig.Fields.Any(p => p.IsSort && p.Name.EqualIgnoreCase(item.ProperyName));

                if (isNotSorteable) continue;

                if (item.OrderType == OrderType.ASC)
                    QueryBuilder.OrderBy(item.ProperyName);
                else
                    QueryBuilder.OrderByDesc(item.ProperyName);

            }
        }

        protected virtual void Filter(IRequestParameterAdapter parameter)
            => FilterByColumnStrategy.FilterByColumn(QueryBuilder, QueryConfig, parameter);

        private void Paginate(IRequestParameterAdapter parameter)
        {
            var page = parameter.Page;
            QueryBuilder.ForPage(page, PerPage);
        }

       


        private async Task<DataResponse<TResult>> QueryData()
        {
            var db = QueryFactorySqlKata.Build(QueryConfig);


            var count = await db.FromQuery(QueryBuilder).CountAsync<int>();
            var data = await db.FromQuery(QueryBuilder).GetAsync<TResult>();


            return new DataResponse<TResult>
            {
                Count = count,
                Data = data
            };
        }


        private DataResponseAbstract<TResult> CreateResponseAdapter(DataResponse<TResult> dataResponse)
        {
            var typeOfResponseType = typeof(DataResponseType);
            var instance = (DataResponseAbstract<TResult>)Activator.CreateInstance(typeOfResponseType, dataResponse);
            return instance;
        }

    }


}
