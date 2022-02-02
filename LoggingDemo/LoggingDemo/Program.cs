using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) //The IHostBuilder method CreateDeafultBuilder is responsible for configuring default logging.
            .ConfigureLogging((context, logging)=> {
                logging.ClearProviders(); //To clear default Logging providers
                logging.AddConfiguration(context.Configuration.GetSection("Logging")); //To add Logging section into configurations
                logging.AddDebug(); //Responsible for addin g logging informationj in debug window
                logging.AddConsole(); //Adding logging information in Console
           
                //There are other logging information offered by default by Microsoft, but we may not need them and only need the above loggings.
                //We can notice how information disappears from the debug or console when we comment their lines and run the application
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
