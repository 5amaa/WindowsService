using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceTutorial.SMS.SmsService
{
    public interface ISmsService
    {
        Task CheckAndSendPendingSms();
    }
}
