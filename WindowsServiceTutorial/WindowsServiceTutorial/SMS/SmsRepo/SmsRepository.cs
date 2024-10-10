using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceTutorial.DTOs;
using WindowsServiceTutorial.SMS.SmsService;

namespace WindowsServiceTutorial.SMS.SmsRepo
{
    public class SmsRepository : ISmsRepository
    {
        private readonly IDbConnection _connectionString;

        public SmsRepository(IDbConnection connectionString)
        {
            _connectionString = connectionString;
        }
        public List<SmsDTO> GetPendingSms()
        {
            const string query = @"SELECT ID AS SmsId, 
                                  MobileNo AS PhoneNumber, 
                                  DESC AS SmsContent
                                  FROM Sms
                                  WHERE Is_Sent = 0";
           
            return _connectionString.Query<SmsDTO>(query).ToList();

              
        }
        public bool MarkSmsAsSent(int smsId)
        {
            const string query = "UPDATE Sms SET IsSent = 1 WHERE SM_ID = @SmsId";
            return _connectionString.Execute(query, new { SmsId = smsId }) > 0;
        }
    }
}
