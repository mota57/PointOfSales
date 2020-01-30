using Microsoft.Data.Sqlite;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.SqlClient;

namespace TablePlugin.Core
{

    public static class QueryFactorySqlKata
    {
        public static QueryFactory Build(QueryConfig queryConfig)
        {
            return Build(queryConfig.Provider, queryConfig.ConnectionString);
        }

        public static QueryFactory Build(DatabaseProvider provider = DatabaseProvider.SQLite, string connectionString = null)
        {
            System.Data.IDbConnection connection = null;
            Compiler compiler = null;

            switch (provider)
            {
                case DatabaseProvider.SQLite:

                    compiler = new SqliteCompiler();
                    connection = new SqliteConnection(connectionString);
                    break;
                case DatabaseProvider.SQLServer:
                    compiler = new SqlServerCompiler();
                    connection = new SqlConnection(connectionString);
                    break;

                default:
                    throw new NotImplementedException();

            }
            var db = new QueryFactory(connection, compiler);
            db.Logger = compiled =>
            {

                Console.WriteLine(compiled.RawSql);
                System.Diagnostics.Debug.WriteLine(compiled.ToString());
            };
            return db;

        }
    }
}
