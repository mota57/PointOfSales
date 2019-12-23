using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TablePlugin.Core
{

    public class RequestTableParameter : IRequestTableParameter
    {
        public string Query { get; set; }
        public int Page { get; set; } = 1;
        public PropertyOrder[] OrderBy { get; set; }
        public bool IsFilterByColumn { get; set; } = true;
    }

    //public class RequestVueParameterAdapter  : IRequestTableParameter
    //{
    //    public RequestVueParameterAdapter(VueTableParameters parameter)
    //    {
    //        _parameter = parameter;
    //    }
    //}

    public enum OrderType { ASC, DESC }

    public class PropertyOrder
    {
        public PropertyOrder()
        {

        }

        public PropertyOrder(string propertyName, OrderType order)
        {
            ProperyName = propertyName;
            OrderType = order;

        }
        public string ProperyName { get; set; }
        public OrderType OrderType { get; set; }
    }


    public class CustomQueryConfig
    {
        public CustomQueryConfig(string tableName, params QueryField[] fields)
        {
            if (tableName.IsBlank()) throw new ArgumentNullException();
            if (fields == null || fields.Length == 0) throw new ArgumentException("fields must contain at least one value");
            TableName = tableName;
            Fields = fields;

        }

        public string TableName { get; }
        public QueryField[] Fields { get; set; }
        private Query query = null;
        protected internal Query Query
        {
            get
            {
                if (query == null)
                {
                    query = new Query(TableName).Select(Fields.Select(p => p.Name).ToArray());
                }
                return query;
            }
        }
        public string ConnectionString { get; set; }
        public DatabaseProvider Provider { get; set; }
    }


  

    public class DataResponse<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }


    public class CustomQueryWithPagination
    {

        public CustomQueryWithPagination()
        {

        }

        private const int DEFAULT_PER_PAGE = 15;
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
        public async Task<dynamic> GetAsync(CustomQueryConfig queryConfig, IRequestTableParameter parameter)
        {
            Query query = queryConfig.Query.Clone();

            QueryFactory db = QueryFactoryBuilder.Build(queryConfig);

            ProcessQuery(query, queryConfig, parameter);

            var count = await db.FromQuery(query).CountAsync<int>();
            var data = await db.FromQuery(query).GetAsync<dynamic>();

            return new
            {
                data,
                count
            };
        }

        public async Task<DataResponse<TData>> GetAsync<TData>(CustomQueryConfig queryConfig, IRequestTableParameter parameter)
        {
            Query query = queryConfig.Query.Clone();

            QueryFactory db = QueryFactoryBuilder.Build(queryConfig);

            ProcessQuery(query, queryConfig, parameter);

            var count = await db.FromQuery(query).CountAsync<int>();
            var data = await db.FromQuery(query).GetAsync<TData>();

            return new DataResponse<TData>
            {
                Data = data,
                Count = count
            };
        }
        private void ProcessQuery(Query query, CustomQueryConfig queryConfig, IRequestTableParameter parameter)
        {
            Filter(query, queryConfig, parameter);
            OrderBy(query, queryConfig, parameter.OrderBy);
            Paginate(query, parameter.Page);
        }


        private void OrderBy(Query query, CustomQueryConfig queryConfig, PropertyOrder[] propertiesOrder)
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

        private void Filter(Query query, CustomQueryConfig queryConfig, IRequestTableParameter parameter)
        {
            if (parameter.IsFilterByColumn)
                FilterByColumn(query, queryConfig, parameter);
            else
                FilterByAllFields(query, queryConfig, parameter);

        }

        private void FilterByColumn(Query query, CustomQueryConfig queryConfig, IRequestTableParameter parameter)
        {
            var queryString = parameter.Query;
            if (parameter.IsFilterByColumn == false || queryString.IsBlank()) return;

            JObject obj = JsonConvert.DeserializeObject<JObject>(queryString);
            foreach (JProperty prop in obj.Properties())
            {
                CheckValidProperty(prop.Name, queryConfig);

                if (!prop.HasValues) continue;
                var name = prop.Name;
                var value = prop.Value.ToString();
                query = query.OrWhereLike(name, $"%{value}%");
            }
        }

        private void FilterByAllFields(Query query, CustomQueryConfig queryConfig, IRequestTableParameter parameter)
        {
            var value = parameter.Query;
            var fields = queryConfig.Fields.Where(p => p.IsFilter);
            foreach (QueryField field in fields)
            {
                query = query.OrWhereLike(field.Name, $"%{value}%");
            }
        }


        private void CheckValidProperty(string prop, CustomQueryConfig config)
        {
            if (!config.Fields.Where(f => f.IsFilter).Any(p => p.Name.EqualIgnoreCase(prop)))
                throw new ArgumentException($"field name {prop} doesn't exists");
        }


        protected internal static class QueryFactoryBuilder
        {
            public static QueryFactory Build(CustomQueryConfig queryConfig)
            {
                return Build(queryConfig.Provider, queryConfig.ConnectionString);
            }

            public static QueryFactory Build(DatabaseProvider provider = DatabaseProvider.SQLite, string connectionString = null)
            {
                System.Data.IDbConnection connection = null;
                Compiler compiler = null;

                switch (provider)
                {
                    case DatabaseProvider.SQLite:

                        compiler = new SqliteCompiler();
                        connection = new SqliteConnection(connectionString);
                        break;
                    case DatabaseProvider.SQLServer:
                        compiler = new SqlServerCompiler();
                        connection = new SqlConnection(connectionString);
                        break;
                }
                var db = new QueryFactory(connection, compiler);
                db.Logger = compiled => {
                    Console.WriteLine(compiled.RawSql);
                    System.Diagnostics.Debug.WriteLine(compiled.ToString());
                };
                return db;

            }

        }
    }
}
