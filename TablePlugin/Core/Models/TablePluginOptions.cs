using System.IO;
using TablePlugin.Data;

namespace TablePlugin.Core
{

    public class TablePluginOptions
    {

        public static DatabaseProvider DatabaseProvider { get; set; }

        public static string SQLConnectionName { get; set; }
        public static string LiteDbConnectionName { get; set; } = "MyData.db";

        public static string RoutePath { get; set; } = "tableplugin";

        private static IQueryRepository _queryRepositoryInstance = null;
        public static IQueryRepository QueryRepositoryInstance
        {
            get
            {
                if (_queryRepositoryInstance != null)
                {
                    return _queryRepositoryInstance;
                }

                _queryRepositoryInstance = new QueryRepository();
                return _queryRepositoryInstance;

            }
            set => _queryRepositoryInstance = value; }

        protected static internal Stream IndexStream()
        {
            var path = UtilHelper.GetFilePath("index.html");
            return System.IO.File.OpenRead(path);
            //return typeof(QueryField).GetType().Assembly.GetManifestResourceStream("TablePlugin.index.html");
        }
    }
}
