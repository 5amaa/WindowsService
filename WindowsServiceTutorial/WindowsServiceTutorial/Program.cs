using Serilog;
using System;
using System.Data.SqlClient;
using System.ServiceProcess;
using WindowsServiceTutorial.Helper;
using WindowsServiceTutorial.SMS.Logging;
using WindowsServiceTutorial.SMS.SmsRepo;
using WindowsServiceTutorial.SMS.SmsService;

namespace WindowsServiceTutorial
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                // Configure Serilog for logging
                ServicesExtentions.SerilogConfiguration();

                Log.Information("Windows Service starting...");

                // Get connection string from environment variables
                var connectionString = Environment.GetEnvironmentVariable("checkbalance", EnvironmentVariableTarget.Machine);
                if (string.IsNullOrEmpty(connectionString))
                {
                    Log.Error("Connection string is not set in environment variables.");
                    return;
                }

                // Check the connection
                SqlConnection sqlConnection = null;
                try
                {
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open(); // Try opening the connection
                    Log.Information("Database connection established successfully.");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Failed to establish a database connection.");
                    return; 
                }
                finally
                {
                    sqlConnection?.Close(); // Close the connection after testing
                }

                // Manually create instances of dependencies using interfaces
                ISmsRepository smsRepository = new SmsRepository(sqlConnection);  // Inject ISmsRepository
                ISmsService smsService = new SmsService(smsRepository);           // Inject ISmsService
                ILogRepository logRepository = new LogRepository(sqlConnection);  // Inject ILogRepository
                ILogService logService = new LogService(logRepository);           // Inject ILogService

                // Set up the services to run
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Service1(smsService, logService) // Inject interfaces into the constructor
                };

                // Start the Windows service
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Windows Service terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush(); // Ensure logs are written before exit
            }
        }
    }
}
