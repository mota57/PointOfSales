using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PointOfSales.Core.Entities;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PointOfSales.Core.Infraestructure.VueTable
{
    ///https://www.laravel-enso.com/examples/table

    public class VueTableParameters
    {
        public string Query { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
        public string OrderBy { get; set; }
        public string Ascending { get; set; }
        public int ByColumn { get; set; }
    }


    public interface IVueTablesInterface
    {
        Task<Dictionary<string, object>> GetAsync(string className, VueTableParameters tableModel, string[] fields);
    }

  
    public class VueTableReader : IVueTablesInterface
    {
        public async Task<Dictionary<string, object>> GetAsync(string className, VueTableParameters tableModel, string[] fields)
        {
            var connection = new SqliteConnection(GlobalVariables.Connection);
            var compiler = new SqlKata.Compilers.SqliteCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Logger = compiled => {
                Console.WriteLine(compiled.ToString());
            };

            var queryBuilder = db.Query(className).Select(fields);


            var count = await db.Query(className).CountAsync<int>();

            if (!string.IsNullOrEmpty(tableModel.Query))
            {
                queryBuilder = tableModel.ByColumn == 1 ?  
                        FilterByColumn(queryBuilder, tableModel.Query)
                        : 
                        Filter(queryBuilder, tableModel.Query, fields);
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
                queryBuilder = queryBuilder.OrWhereLike(prop.Name, $"%{value}%");
            }
            return queryBuilder;

        }

        private Query Filter(Query queryBuilder, string query, string[] fields)
        {
            foreach(string field in fields)
            {
                queryBuilder = queryBuilder.OrWhereLike(field, $"%{query}%");
            }
            return queryBuilder;
        }

    }
}
