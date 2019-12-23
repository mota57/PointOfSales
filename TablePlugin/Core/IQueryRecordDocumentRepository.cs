using System.Collections.Generic;

namespace TablePlugin.Core
{
    public interface IQueryRecordDocumentRepository
    {
        string ConnectionName { get; set; }

        void Delete(int id);
        IEnumerable<QueryRecordDocument> GetAll();
        CustomQueryConfig GetByConfig(string configName);
        QueryRecordDocument LoadRecordById(int id);
        void Upsert(QueryRecordDocument record);
    }
}