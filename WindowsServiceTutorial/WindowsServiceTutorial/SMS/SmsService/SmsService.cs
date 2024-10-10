using PremiumMainCS.PublicFunction;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceTutorial.DTOs;

namespace WindowsServiceTutorial.SMS.SmsService
{
    public class SmsService : ISmsService
    {
        private readonly ISmsRepository _smsRepository;

        public SmsService(ISmsRepository smsRepository)
        {
            _smsRepository = smsRepository;
        }

        public async Task CheckAndSendPendingSms()
        {
            try
            {

                List<SmsDTO> pendingSmsRecords = _smsRepository.GetPendingSms();

                if (!pendingSmsRecords.Any())
                {
                    Log.Information($"CreationTime :{DateTime.Now} No pending SMS found.");
                    return;
                }

                foreach (var sms in pendingSmsRecords)
                {

                    Log.Information($"CreationTime :{DateTime.Now} Attempting to send SMS to {sms.PhoneNumber}.");

                    if (string.IsNullOrEmpty(sms.PhoneNumber) || string.IsNullOrEmpty(sms.SmsContent))
                    {
                        Log.Error($"CreationTime :{DateTime.Now} The Phone Number Or SMS Content Is Null  with Id {sms.SmsId}. ");
                        return;
                    }

                    bool isSent = await SendSmsAsync(sms.PhoneNumber, sms.SmsContent);

                    if (isSent)
                    {
                        Log.Information($"CreationTime :{DateTime.Now} SMS with Id {sms.SmsId} sent successfully to {sms.PhoneNumber} .");
                        bool isMark = _smsRepository.MarkSmsAsSent(sms.SmsId); // Mark as sent in the database
                        if (!isMark)
                        {
                            Log.Error($"CreationTime : {DateTime.Now} Failed to mark SMS As Sent to {sms.PhoneNumber}  with Id {sms.SmsId}.");
                        }
                        else { Log.Information($"CreationTime :{DateTime.Now} Mark SMS As Sent to {sms.PhoneNumber} with Id {sms.SmsId}."); }
                    }
                    else
                    {
                        Log.Error($"CreationTime : {DateTime.Now} Failed to send SMS to {sms.PhoneNumber}  with Id {sms.SmsId}.");
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error($"CreationTime : {DateTime.Now} Failed to send SMS to  , Message: {ex} ");

            }
        }
        
    

        private async Task<bool> SendSmsAsync(string phoneNumber, string smsContent)
         {
             try
             {
                 
                 return true;

             }
             catch (Exception ex)
             {
                Log.Error($"CreationTime : {DateTime.Now} Failed to send SMS to {phoneNumber} , Message: {ex} ");
                return false;
             }
         }
        
    }
    
}
