using LiteDB;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TablePlugin.Data;

namespace TablePlugin.Core
{

    public class QueryRepository : IQueryRepository
    {

        private static ConcurrentDictionary<string, QueryConfig> ConfigCache = 
            new ConcurrentDictionary<string, QueryConfig>();

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

        public QueryRepository(string connectionName = null)
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
                ConfigCache.Clear();
            }
        }
        public void Delete(int id)
        {

            using (var db = new LiteRepository(ConnectionName))
            {
                db.Delete<QueryRecordDocument>(id);
                ConfigCache.Clear();
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

        public QueryConfig GetByConfig(string configName)
        {
            if (ConfigCache.ContainsKey(configName))
                return ConfigCache[configName];

            using (var db = new LiteDatabase(ConnectionName))
            {
                var result = db.GetCollection<QueryRecordDocument>()
                             .FindOne(Query.EQ("ConfigName", configName));

                var queryConfig = MapToQueryConfig(result);

                if (!ConfigCache.TryAdd(configName, queryConfig))
                    throw new System.Exception("could not add the new configuration at ");
            }
            return ConfigCache[configName];
        }




        private QueryConfig MapToQueryConfig(QueryRecordDocument queryRecord)
        {
            var queryFields = queryRecord.QueryFieldDocuments
           .Select(q => new QueryField(q.Name, q.IsFilter, q.IsSort, q.Display, q.FriendlyName, null))
           .ToArray();

            var config = new QueryConfig(queryRecord.TableName, queryFields);
            return config;
        }
    }

}
