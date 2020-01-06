using SqlKata.Execution;
using System.Collections.Generic;
using System.Linq;

namespace TablePlugin.Core
{
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
}
