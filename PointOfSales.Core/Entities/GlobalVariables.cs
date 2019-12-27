namespace PointOfSales.Core.Entities
{

    public static class GlobalVariables
    {
        public static string Connection { get; set; }
        
        public static DatabaseProvider DatabaseProvider => DatabaseProvider.SQLite;

    }


}
