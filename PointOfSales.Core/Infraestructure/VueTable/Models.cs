using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PointOfSales.Core.Entities;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
        Task<Dictionary<string, object>> GetAsync(VueTableConfig config, VueTableParameters parameters);
    }

    public class VueField
    {
        public VueField(string name, bool filter = true)
        {
            if (string.IsNullOrEmpty(name)) {
                throw new Exception("field must have a name");
            }
            this.Name = name;
            this.Filter = filter;
        }

        public string Name { get; set; }
        public bool Filter { get; set; } = true;
        public bool Display { get; set; } = true;
    }

    public class VueTableConfig
    {
        public VueTableConfig()
        {
            Fields = new List<VueField>();

        }

        public string TableName { get; set; }
        public List<VueField> Fields { get; set; } 
        
    }



    public class VueTableReader : IVueTablesInterface
    {
        public async Task<Dictionary<string, object>> GetAsync(VueTableConfig config, VueTableParameters parameters)
        {
            var tableName = config.TableName;
            var fields = config.Fields;
            var fieldNames = fields.Select(_ => _.Name).ToArray();

            QueryFactory db = BuildQueryFactory();

            var queryBuilder = db.Query(tableName).Select(fieldNames);


            var count = await db.Query(tableName).CountAsync<int>();

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

        private QueryFactory BuildQueryFactory()
        {
            System.Data.IDbConnection connection;
            Compiler compiler;

            if (GlobalVariables.DatabaseProvider == DatabaseProvider.SQLite)
            {
                compiler = new SqliteCompiler();
                connection = new SqliteConnection(GlobalVariables.Connection);
            }
            else 
            {
                compiler = new SqlServerCompiler();
                connection = new SqlConnection(GlobalVariables.Connection);
            }

            var db = new QueryFactory(connection, compiler);
            db.Logger = compiled => {
                System.Diagnostics.Debug.WriteLine(compiled.ToString());
            };
            return db;

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
