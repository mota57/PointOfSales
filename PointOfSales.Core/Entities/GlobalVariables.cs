namespace PointOfSales.Core.Entities
{

    public static class GlobalVariables
    {
        public static string Connection => @"Data Source=C:\Users\hmota\Documents\RESOURCES\Projects\PointOfSales\PointOfSales.Core\pos.db";
        //public static string Connection => @"Data Source=C:\Users\hpnotebook\Documents\Visual Studio 2017\Projects\PointOfSales\PointOfSales.Core\pos.db";

        public static DatabaseProvider DatabaseProvider => DatabaseProvider.SQLite;

    }


}
