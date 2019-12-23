using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TablePlugin.Data;

namespace TablePlugin.Core
{

    public class QueryRecordDocumentRepository : IQueryRecordDocumentRepository
    {
        private string _connectionName = null;
        public string ConnectionName
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionName))
                {
                    return TablePluginOptions.LiteDbConnectionName;
                }
                return _connectionName;
            }
            set
            {
                _connectionName = null;
            }
        }

        public QueryRecordDocumentRepository(string connectionName = null)
        {
            ConnectionName = connectionName;
        }


        public QueryRecordDocument LoadRecordById(int id)
        {
            using (var db = new LiteDatabase(ConnectionName))
            {
                var col = db.GetCollection<QueryRecordDocument>()
                            .FindById(id);
                return col;
            }
        }

        public void Upsert(QueryRecordDocument record)
        {
            using (var db = new LiteRepository(ConnectionName))
            {
                db.Upsert(record);
            }
        }
        public void Delete(int id)
        {

            using (var db = new LiteRepository(ConnectionName))
            {
                db.Delete<QueryRecordDocument>(id);
            }
        }
        public IEnumerable<QueryRecordDocument> GetAll()
        {
            using (var db = new LiteDatabase(ConnectionName))
            {
                var col = db.GetCollection<QueryRecordDocument>()
                            .FindAll();
                return col;
            }
        }

        public CustomQueryConfig GetByConfig(string configName)
        {
            using (var db = new LiteDatabase(ConnectionName))
            {
                var result = db.GetCollection<QueryRecordDocument>()
                             .FindOne(Query.EQ("ConfigName", configName));

                var queryFields = result.QueryFieldDocuments
               .Select(q => new QueryField(q.Name, q.IsFilter, q.IsSort, q.Display, q.FriendlyName, q.Type))
               .ToArray();

                return new CustomQueryConfig(result.TableName, queryFields);
            }
        }

      
    }

}
