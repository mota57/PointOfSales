using LiteDB;
using System.Collections.Generic;

namespace TablePlugin.Data
{
    public class QueryRecordDocument
    {
        [BsonId(true)]
        public int Id { get; set; }
        public string ConfigName { get; set; }
        public string TableName { get; set; }
        //[BsonRef("QueryFieldDocuments")]
        public ICollection<QueryFieldDocument> QueryFieldDocuments { get; set; }
    }

}
