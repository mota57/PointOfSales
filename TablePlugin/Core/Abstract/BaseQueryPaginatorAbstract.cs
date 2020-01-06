using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePlugin.Core
{
    public abstract class BaseQueryPaginatorAbstract : IQueryPaginator
    {
        public BaseQueryPaginatorAbstract(QueryConfig queryConfig)
        {
            this.QueryConfig = queryConfig;
        }

        private const int DEFAULT_PER_PAGE = 10;
        private int _perPage = DEFAULT_PER_PAGE;

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


        public async Task<object> GetAsync(IRequestParameter request)
        {
            ProcessQuery(request);
            return await BuildResponseAsync<object>();
        }

        public async Task<DataResponse<TData>> GetAsync<TData>(IRequestParameter request)
        {
            ProcessQuery(request);
            return await BuildResponseAsync<TData>();
        }
       

        protected void ProcessQuery(IRequestParameter parameter)
        {
            QueryBuilder = QueryConfig.Query.Clone();
            PerPage = parameter.PerPage;

            Filter(parameter);
            OrderBy(parameter);
            Paginate(parameter);
        }

        protected virtual void Paginate(IRequestParameter parameter)
        {
            var page = parameter.Page;
            QueryBuilder.ForPage(page, PerPage);
        }
        protected virtual void OrderBy(IRequestParameter parameter)
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

        protected virtual void Filter(IRequestParameter parameter)
        {
                new BasicFilterByColumnStrategy()
                .FilterByColumn(QueryBuilder, QueryConfig, parameter);
        }


        public virtual async Task<DataResponse<TData>> BuildResponseAsync<TData>()
        {

            var db = QueryFactorySqlKata.Build(QueryConfig);


            var count = await db.FromQuery(QueryBuilder).CountAsync<int>();
            var data = await db.FromQuery(QueryBuilder).GetAsync<TData>();

            return new DataResponse<TData>
            {
                Count = count,
                Data = data
            };

        }

     
    }
}
