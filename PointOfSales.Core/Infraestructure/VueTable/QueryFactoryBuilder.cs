using System;
using SqlKata.Execution;
using SqlKata.Compilers;
using PointOfSales.Core.Entities;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;

namespace PointOfSales.Core.Infraestructure.VueTable
{


    public static class  QueryFactoryBuilder 
    {
        public static QueryFactory BuildQueryFactory(DatabaseProvider provider = DatabaseProvider.SQLite)
        {
            System.Data.IDbConnection connection = null;
            Compiler compiler = null;
            
            switch(provider)
            {
                case DatabaseProvider.SQLite:
                
                    compiler = new SqliteCompiler();
                    connection = new SqliteConnection(GlobalVariables.Connection);
                break;
                case DatabaseProvider.SQLServer:   
                    compiler = new SqlServerCompiler();
                    connection = new SqlConnection(GlobalVariables.Connection);
                break;
            }
            var db = new QueryFactory(connection, compiler);
            db.Logger = compiled => {
                Console.WriteLine(compiled.RawSql);
                System.Diagnostics.Debug.WriteLine(compiled.ToString());
            };
            return db;

        }
 
    }
}
