using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
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

    public class QueryConfigDTO
    {
        public QueryConfigDTO(QueryConfig config)
        {
            this.TableName = config.TableName;
            this.Fields = config.Fields.Select(v => new QueryFieldDTO(v));

        }

        public string TableName { get; }
        public IEnumerable<QueryFieldDTO> Fields { get; }
    }

    public class QueryFieldDTO
    {
        public QueryFieldDTO(IQueryField queryField)
        {
            this.Name = queryField.Name;
            this.Filter = queryField.IsFilter;
            this.Display = queryField.Display;
            this.Type = queryField.Type?.Name;
        }

        public string Name { get; set; }
        public bool Filter { get; set; }
        public bool Display { get; set; }

        public string Type { get; set; }

    }
}
