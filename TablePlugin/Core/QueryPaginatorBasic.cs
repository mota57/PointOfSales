using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TablePlugin.Data;

namespace TablePlugin.Core
{

    public partial class QueryPaginatorBasic
    {

        public IFilterByColumnStrategy FilterByColumnStrategy { get; set; } = new BasicFilterByColumnStrategy();

        public QueryPaginatorBasic()
        {

        }

        public QueryPaginatorBasic(IFilterByColumnStrategy filterByColumnStrategy)
        {
            FilterByColumnStrategy = filterByColumnStrategy;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryConfig"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(QueryConfig queryConfig, IRequestParameter parameter)
        {

            Query query = queryConfig.Query.Clone();

            QueryFactory db = QueryFactorySqlKata.Build(queryConfig);

            ProcessQuery(query, queryConfig, parameter);

            var count = await db.FromQuery(query).CountAsync<int>();
            var data = await db.FromQuery(query).GetAsync<object>();

            return new
            {
                data,
                count
            };
        }

        public async Task<DataResponse<TData>> GetAsync<TData>(QueryConfig queryConfig, IRequestParameter parameter)
        {
            Query query = queryConfig.Query.Clone();

            QueryFactory db = QueryFactorySqlKata.Build(queryConfig);

            ProcessQuery(query, queryConfig, parameter);

            var count = await db.FromQuery(query).CountAsync<int>();
            var data = await db.FromQuery(query).GetAsync<TData>();

            return new DataResponse<TData>
            {
                Data = data,
                Count = count
            };
        }



        private void ProcessQuery(Query query, QueryConfig queryConfig, IRequestParameter parameter)
        {

            PerPage = parameter.PerPage;
            Filter(query, queryConfig, parameter);
            OrderBy(query, queryConfig, parameter.OrderBy);
            Paginate(query, parameter.Page);
        }


        private void OrderBy(Query query, QueryConfig queryConfig, PropertyOrder[] propertiesOrder)
        {
            if (propertiesOrder == null) return;

            foreach (var item in propertiesOrder)
            {
                var isNotSorteable = !queryConfig.Fields.Any(p => p.IsSort && p.Name.EqualIgnoreCase(item.ProperyName));

                if (isNotSorteable) continue;

                if (item.OrderType == OrderType.ASC)
                    query = query.OrderBy(item.ProperyName);
                else
                    query = query.OrderByDesc(item.ProperyName);

            }
        }


        private void Paginate(Query queryBuilder, int page)
              => queryBuilder = queryBuilder.ForPage(page, PerPage);

        private void Filter(Query query, QueryConfig queryConfig, IRequestParameter parameter)
        {

            if (parameter.IsFilterByColumn == false || parameter.Query == null || parameter.Query.Count() == 0) return;

            if (parameter.IsFilterByColumn)
                FilterByColumnStrategy.FilterByColumn(query, queryConfig, parameter);
            else
                FilterByAllFields(query, queryConfig, parameter);

        }
        

       
       

        


        private void FilterByAllFields(Query query, QueryConfig queryConfig, IRequestParameter parameter)
        {
            var value = parameter.Query;
            var fields = queryConfig.Fields.Where(p => p.IsFilter);
            foreach (QueryField field in fields)
            {
                query = query.OrWhereLike(field.Name, $"%{value}%");
            }
        }


        private void CheckValidProperty(string prop, QueryConfig config)
        {
            if (!config.Fields.Where(f => f.IsFilter).Any(p => p.Name.EqualIgnoreCase(prop)))
                throw new ArgumentException($"field name {prop} doesn't exists");
        }
    }
}
