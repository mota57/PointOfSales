using System.IO;
using Microsoft.Extensions.Configuration;
using System;
namespace PointOfSales.Core.Infraestructure
{
    public class ConfigurationLoaderHelper 
    {
        public ConfigurationLoaderHelper()
        {
            Build();
        }

        public IConfigurationRoot Config {get; private set;}
        private void Build()
        {

            var parentDirectory = Directory.GetParent (Directory.GetCurrentDirectory ()).FullName;
            var webDirectory = Path.Combine(parentDirectory, "PointOfSales.WebUI");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("parentDirectory {0}\nWebDirectory{1}", parentDirectory, webDirectory);
            Console.ResetColor();

             Config = new ConfigurationBuilder()
            .SetBasePath(webDirectory)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();
        }
    }

}
