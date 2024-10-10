using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceTutorial.SMS.Logging
{
    public class LogRepository:ILogRepository
    {
        private readonly IDbConnection _connectionString;

        public LogRepository(IDbConnection connectionString)
        {
            _connectionString = connectionString;
        }

        public void LogServiceStatus(string serviceName, string statusMessage)
        {
            try
            {
                const string query = "INSERT INTO SmsTable (Status, Message) VALUES (@ServiceName, @StatusMessage)";

                _connectionString.Execute(query, new { ServiceName = serviceName, StatusMessage = statusMessage });

            }
            catch (Exception ex)
            {
                Log.Error($"CreationTime : {DateTime.Now} Failed to Log Service Status  , Message: {ex} ");

            }


        }
    }
}
