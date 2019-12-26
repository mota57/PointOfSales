﻿using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TablePlugin.Data;

namespace TablePlugin.Core
{
    public class QueryPaginatorBasic
    {

        public QueryPaginatorBasic()
        {
            
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
        public async Task<object> GetAsync(QueryConfig queryConfig, IRequestTableParameter parameter)
        {

            Query query = queryConfig.Query.Clone();

            QueryFactory db = QueryFactoryBuilder.Build(queryConfig);

            ProcessQuery(query, queryConfig, parameter);

            var count = await db.FromQuery(query).CountAsync<int>();
            var data = await db.FromQuery(query).GetAsync<object>();

            return new
            {
                data,
                count
            };
        }

        public async Task<DataResponse<TData>> GetAsync<TData>(QueryConfig queryConfig, IRequestTableParameter parameter)
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



        private void ProcessQuery(Query query, QueryConfig queryConfig, IRequestTableParameter parameter)
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

        private void Filter(Query query, QueryConfig queryConfig, IRequestTableParameter parameter)
        {
            if (parameter.IsFilterByColumn)
                FilterByColumn(query, queryConfig, parameter);
            else
                FilterByAllFields(query, queryConfig, parameter);

        }

        private void FilterByColumn(Query query, QueryConfig queryConfig, IRequestTableParameter parameter)
        {
            var queryString = parameter.Query;
            if (parameter.IsFilterByColumn == false || queryString.IsBlank()) return;

            QueryFilter[] obj = JsonConvert.DeserializeObject<QueryFilter[]>(queryString);
            foreach (QueryFilter prop in obj)
            {
                if (prop.Value == null) continue;
                var name = prop.Name;
                var value = prop.Value;

                switch (prop.Operator)
                {
                    case OperatorType.Contains:
                        query.OrWhereContains(name, value.ToString());
                        break;

                    case OperatorType.EndWith:
                        query.OrWhereEnds(name, value.ToString());
                        break;

                    case OperatorType.StartWith:
                        query.OrWhereStarts(name, value.ToString());
                        break;


                    case OperatorType.Equals:
                        query.OrWhere(name, "=", value);
                        break;

                    case OperatorType.NotEquals:
                        query.OrWhere(name, "!=", value);
                        break;

                    case OperatorType.LessThan:
                        query.OrWhere(name, "<", value);
                        break;


                    case OperatorType.LessOrEqual:
                        query.OrWhere(name, "<=", value);
                        break;


                    case OperatorType.GreaterThan:
                        query.OrWhere(name, ">", value);
                        break;


                    case OperatorType.GreaterOrEqual:
                        query.OrWhere(name, ">=", value);
                        break;

                    case OperatorType.DateWithoutTime:
                        query.OrWhereDate(name, prop.DateLogicalOperator, value);
                    break;
                }
            }
        }


        private void FilterByAllFields(Query query, QueryConfig queryConfig, IRequestTableParameter parameter)
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


        protected internal static class QueryFactoryBuilder
        {
            public static QueryFactory Build(QueryConfig queryConfig)
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
                db.Logger = compiled =>
                {

                    Console.WriteLine(compiled.RawSql);
                    System.Diagnostics.Debug.WriteLine(compiled.ToString());
                };
                return db;

            }

        }
    }
}