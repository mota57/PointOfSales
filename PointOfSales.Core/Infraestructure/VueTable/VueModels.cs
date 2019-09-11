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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filter"></param>
        /// <param name="sqlField">Must set SqlField property in class VueField when using inline query is not null</param>
        public VueField(string name,  bool filter = true, string sqlField = "")
        {
            if (string.IsNullOrEmpty(name)) {
                throw new Exception("field must have a name");
            }
            this.Name = name;
            this.Filter = filter;
            this.SqlField = sqlField;
        }

        public string Name { get; set; }
        public bool Filter { get; set; } = true;
        public bool Display { get; set; } = true;
        /// <summary>
        /// Must set SqlField property when VueTableConfig.QueryBuilder is not null.  
        /// </summary>
        public string SqlField { get; set; }
    }

    public class VueTableConfig
    {

        public VueTableConfig()
        {
            Fields = new List<VueField>();

        }

        /// <summary>
        ///  Must set SqlField property when VueTableConfig.QueryBuilder is not null. 
        /// </summary>
        public Query QueryBuilder { get; set; } = null;


        public string TableName { get; set; }
        public List<VueField> Fields { get; set; } 
        
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

            QueryFactory db = BuildQueryFactory();

            Query queryBuilder = null;
            if(config.QueryBuilder == null)
            {
                queryBuilder = db.Query(tableName).Select(fieldNames);
            }
            else
            {
                queryBuilder = db.FromQuery(config.QueryBuilder);
                IsCustomInlineQuery = true;
                if(fields.Any(_ => string.IsNullOrEmpty(_.SqlField))){
                    throw new Exception("Must set SqlField property in class VueField whenVueTableConfig.QueryBuilder is not null");
                }
                MapFieldSql = new Dictionary<string, string>(fields.Select(_ => new KeyValuePair<string, string>(_.Name, _.SqlField)));
            }


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
