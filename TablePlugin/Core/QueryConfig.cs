using SqlKata;
using SqlKata.Execution;
using System;
using System.Linq;

namespace TablePlugin.Core
{
    public class QueryConfig 
    {
        public QueryConfig(string tableName, params QueryField[] fields)
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
}
