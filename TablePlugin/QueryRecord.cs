using System.Collections.Generic;

namespace TablePlugin
{
    public class QueryRecord
    {
        public int Id { get; set; }
        public string ConfigName { get; set; }
        public string TableName { get; set; }
        public ICollection<QueryField> QueryFields { get; set; }
    }
}
