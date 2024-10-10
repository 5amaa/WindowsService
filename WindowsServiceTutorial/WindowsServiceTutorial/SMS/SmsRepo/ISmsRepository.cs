using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceTutorial.DTOs;

namespace WindowsServiceTutorial.SMS.SmsService
{
    public interface ISmsRepository
    {
        List<SmsDTO> GetPendingSms();
        bool MarkSmsAsSent(int smsId);
    }
}
