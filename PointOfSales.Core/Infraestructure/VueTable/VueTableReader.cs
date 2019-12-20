using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlKata.Execution;
using SqlKata;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PointOfSales.Core.Entities;
using SqlKata.Compilers;
using Microsoft.Data.Sqlite;
using System;
using System.Data.SqlClient;

namespace PointOfSales.Core.Infraestructure.VueTable
{

    public static class QueryFactoryBuilder
    {
       
        public static QueryFactory BuildQueryFactory() =>
             BuildQueryFactory(GlobalVariables.DatabaseProvider, GlobalVariables.Connection);

        public static QueryFactory BuildQueryFactory(DatabaseProvider provider, string connectionString)
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
    public class VueTableReader : IVueTablesInterface
    {
        private Dictionary<string, string> MapFieldSql;
        private bool IsCustomInlineQuery = false;


        public async Task<Dictionary<string, object>> GetAsync(VueTableConfig config, VueTableParameters parameters)
        {
            var tableName = config.TableName;
            var fields = config.Fields;
            var fieldNames = fields.Select(_ => _.Name).ToArray();

            QueryFactory db = QueryFactoryBuilder.BuildQueryFactory();

            Query queryBuilder = null;
            if(config.QueryBuilder == null)
            {
                queryBuilder = db.Query(tableName).Select(fieldNames);
            }
            else
            {                        
                IsCustomInlineQuery = true;
                queryBuilder = db.FromQuery(config.QueryBuilder);
                MapFieldSql = new Dictionary<string, string>(fields.Select(_ =>
                                new KeyValuePair<string, string>(_.Name,  _.SqlField )));
            }


            //var count = await db.Query(tableName).CountAsync<int>();
            var count = await db.FromQuery(queryBuilder).CountAsync<int>();

            if (!string.IsNullOrEmpty(parameters.Query))
            {
                queryBuilder = parameters.ByColumn == 1 ?  
                        FilterByColumn(queryBuilder, parameters.Query)
                        : 
                        Filter(queryBuilder, parameters.Query, fieldNames);
            }

            var data = await queryBuilder.GetAsync<object>();



            return new  Dictionary<string, object>
            {
                { "data", data },
                { "count", count }
            };
        }

       
        private Query FilterByColumn(Query queryBuilder, string query)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(query);
            foreach(JProperty prop in obj.Properties())
            {
                if (!prop.HasValues)
                {
                    continue;
                }
                var value = prop.Value.ToString();
                queryBuilder = queryBuilder.OrWhereLike( IsCustomInlineQuery ? MapFieldSql[prop.Name] : prop.Name, $"%{value}%");
            }
            return queryBuilder;

        }

        private Query Filter(Query queryBuilder, string query, string[] fields)
        {
            foreach(string field in fields)
            {
                queryBuilder = queryBuilder.OrWhereLike(IsCustomInlineQuery ? MapFieldSql[field] : field, $"%{query}%");
            }
            return queryBuilder;
        }
      
    }
}
