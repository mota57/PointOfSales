using Microsoft.Data.Sqlite;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.SqlClient;

namespace TablePlugin
{
    public static class QueryFactoryBuilder
    {
        public static QueryFactory BuildQueryFactory(CustomQueryConfig queryConfig)
        {
            return BuildQueryFactory(queryConfig.Provider, queryConfig.ConnectionString);
        }

        public static QueryFactory BuildQueryFactory(DatabaseProvider provider = DatabaseProvider.SQLite, string connectionString = null)
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
