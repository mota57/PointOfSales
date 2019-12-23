using System.Collections.Generic;
using TablePlugin.Data;

namespace TablePlugin.Core
{
    public interface IQueryRepository
    {
        string ConnectionName { get; set; }
        void Delete(int id);
        IEnumerable<QueryRecordDocument> GetAll();
        QueryConfig GetByConfig(string configName);
        QueryRecordDocument LoadRecordById(int id);
        void Upsert(QueryRecordDocument record);
    }
}