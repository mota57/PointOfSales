using SqlKata;
using SqlKata.Execution;
using System;
using System.Linq;

namespace TablePlugin.Core
{
    public class QueryConfig 
    {
        public QueryConfig(string tableName, params IQueryField[] fields)
        {
            if (tableName.IsBlank()) throw new ArgumentNullException();
            if (fields == null || fields.Length == 0) throw new ArgumentException("fields must contain at least one value");
            TableName = tableName;
            Fields = fields;

        }

        public string TableName { get; }
        public IQueryField[] Fields { get; set; }

        public IQueryField GetQueryFieldByName(string name) => Fields.FirstOrDefault(p => p.Name.EqualIgnoreCase(name));
       

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

   
}
