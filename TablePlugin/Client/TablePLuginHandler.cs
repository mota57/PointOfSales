using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using TablePlugin.Core;
using TablePlugin.Data;

namespace TablePlugin.Client
{
    public class TablePLuginHandler
    {
  

        MainRequestHandler Handler;

        IQueryRecordDocumentRepository QueryRepository;

        public TablePLuginHandler(MainRequestHandler handler, IQueryRecordDocumentRepository queryRepository)
        {
            Handler = handler;
            QueryRepository = queryRepository;
        }

        public async Task IndexPage()
        {
            using (var stream = TablePluginOptions.IndexStream())
            {
                var htmlBuilder = new StringBuilder(new StreamReader(stream).ReadToEnd());
                foreach (var entry in GetIndexArguments())
                {
                    htmlBuilder.Replace(entry.Key, entry.Value);
                }
                await Handler.Ok(htmlBuilder.ToString());
            }
        }

        public async Task Load()
        {
             await Handler.Ok(QueryRepository.GetAll());  
        }

        public async Task LoadRecordById()
        {
            var id = Handler.Request.Query.Get<int>("id");
            using (var db = new LiteDatabase(TablePluginOptions.LiteDbConnectionName))
            {
                var col = db.GetCollection<QueryRecordDocument>()
                            .FindById(id);
                await Handler.Ok(col);
            }
        }

        public async Task Upsert()
        {

            var record = Handler.ParseBody<QueryRecordDocument>();

            using (var db = new LiteRepository(TablePluginOptions.LiteDbConnectionName))
            {
                db.Upsert(record);
            }
            await Handler.Ok();
        }
        public async Task Delete()
        {
            var id = this.Handler.Request.Query.Get<int>("id");
            
            using (var db = new LiteRepository(TablePluginOptions.LiteDbConnectionName))
            {
                db.Delete<QueryRecordDocument>(id);
            }
            await Handler.Ok();
        }

        private IDictionary<string, string> GetIndexArguments()
        {
            return new Dictionary<string, string>()
            {
                { "%(BaseUrl)", Handler.AppBaseUrl() },
            };
        }

        
    }
}
