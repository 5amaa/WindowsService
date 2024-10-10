using Dapper;
using PremiumMainCS.PublicFunction;
using Serilog;
using System;
using System.Data;
using System.Threading.Tasks;
using WindowsServiceTutorial.SMS.Logging;
using WindowsServiceTutorial.SMS.SmsService;

namespace WindowsServiceTutorial.SMS.SmsRepo
{
    public class LogService: ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public async Task RunDailyCheckAsync()
        {
            try
            {
                Log.Information("SMS Running daily check at 12 AM.");


                // Log the success status in the database
                _logRepository.LogServiceStatus("SMS Windows Service", $"Service running well. at {DateTime.Now} SMS sent today.");

                
            }
            catch (Exception ex)
            {
                // Log any errors in the database
                Log.Error($"Error during daily check: {ex.Message}");

                _logRepository.LogServiceStatus("SMS Windows Service", $"Error during daily check: {ex.Message} at {DateTime.Now}");
            }
        }
    }
}
