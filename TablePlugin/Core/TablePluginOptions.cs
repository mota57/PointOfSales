using System.IO;
using TablePlugin.Data;

namespace TablePlugin.Core
{

    //public class PluginOptions
    //{

    //    public  DatabaseProvider DatabaseProvider { get; set; }

    //    public  string SQLConnectionName { get; set; }


    //    public  string RoutePath { get; set; } = "tableplugin";
    //}

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
            //return System.IO.File.OpenRead(@"C:\Users\hpnotebook\Documents\Visual Studio 2017\Projects\PointOfSales\TablePlugin\index.html");
            return System.IO.File.OpenRead(@"C:\Users\hmota\Desktop\GLOBAL CRM\RESOURCES\Projects\PointOfSales\TablePlugin\index.html");
            //return typeof(QueryField).GetType().Assembly.GetFile("TablePlugin.index.html");
            //return typeof(QueryField).GetType().Assembly.GetManifestResourceStream("TablePlugin.index.html");
        }
    }
}
