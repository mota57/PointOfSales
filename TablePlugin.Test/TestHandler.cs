using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TablePlugin.Test
{
    public class TestHandler {

        public static string  Connection {get; set;}
        public static void Handle<TContext>(Action<DbContextOptions<TContext>> action)
            where TContext : DbContext 
        {
            var connection = new SqliteConnection(Connection);
            connection.Open();
            
            var options = new DbContextOptionsBuilder<TContext>()
                .UseSqlite(connection)
                .Options;
            
            
            try
            {
                using (var context = (TContext) Activator.CreateInstance(typeof(TContext), options))
                {
                    //create database
                    context.Database.EnsureCreated();
                    
                }
                action(options);
            
            } finally
            {
                connection.Close();
            }
        }
    }
}
