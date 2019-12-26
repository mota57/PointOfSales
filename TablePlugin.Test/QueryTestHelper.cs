using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace TablePlugin.Test
{
    public static class QueryTestHelper {

        public static ILogger<T> GetLogger<T> () {

            var serviceProvider = new ServiceCollection ()
                .AddLogging (builder => {
                    builder.AddDebug ()
                        .AddConsole ();
                }).BuildServiceProvider ();
            return serviceProvider.GetService<ILogger<T>> ();
        }

        public static void LogToFile<T> (object result) {

            LogToFile<T> (JsonConvert.SerializeObject (result));
        }

        public static void LogToFile<T> (string result) {
            var stackTrace = new StackTrace();
            var name = typeof (T).FullName;
            var date = DateTime.Now.ToString("hh:mm:ss MM/dd/YYYY");
            string methodCaller = stackTrace.GetFrame(1).GetMethod().Name;
            // Get calling method name
            if(methodCaller.Contains("LogToFile"))
                methodCaller = stackTrace.GetFrame(2).GetMethod().Name;
            
            System.IO.File.AppendAllText
            (
                UtilHelper.GetFilePath ("output.txt"), 
                $"\n\n\n\nFileName: {name}.{methodCaller} at time: {date}\n{result}"
            );
        }
    }
}