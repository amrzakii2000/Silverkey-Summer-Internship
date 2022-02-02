using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        //We can create our custom logger
        public IndexModel(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger("CustomLogger");
        }

        public void OnGet()
        {
            _logger.LogInformation("Hello Logging");                //Logs Information and mainly in how application can be used
            _logger.LogInformation(101, "Hello logging with ID");   //We can have a LogID
            _logger.LogTrace("Tracing Log");                        //can be used to give detailed view and application secrets
            _logger.LogError("Error Log");                          //used for logging errors and exceptions that can crash the application
            _logger.LogDebug("Debug Log");                          //can be used for logging debugging information
            _logger.LogWarning("Warning Log");                      //used for logging warning like when we catch exceptions
            _logger.LogCritical("Critical Log");                    //used for logging critical crashes and errors

            //We can use variables with log messages
            _logger.LogError("App crashes at {Time}", DateTime.UtcNow);

            //Can we just write the previous line as follows ?
            _logger.LogError($"App crashes at {DateTime.UtcNow}");

            //Well, that works. But, the previous examples is a better apporoach. It is better to specify variables in the arguments as some logging frameworks store these variables in json files.
            //So, it will be helpful in many cases like when we search for a log message at a specific date or time.

            // We can use logging with exceptions
            try
            {
                throw new Exception("Test exception");
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex, "Catching exception");
            }

        }
    }
}
//Logging is responsible for logging information into the console.
