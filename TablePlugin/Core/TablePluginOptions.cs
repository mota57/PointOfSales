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
        public static IQueryRecordDocumentRepository QueryDocumnetInstance { get;  set; }

        protected static internal Stream IndexStream()
        {
            return System.IO.File.OpenRead(@"C:\Users\hpnotebook\Documents\Visual Studio 2017\Projects\PointOfSales\TablePlugin\index.html");
            //return typeof(Extensions)
            //.GetTypeInfo()
            //.Assembly
            //.GetManifestResourceStream("TablePlugin.index.html");
       }
    }
}
