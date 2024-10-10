using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceTutorial.SMS.Logging
{
    public interface ILogRepository
    {
        void LogServiceStatus(string serviceName, string statusMessage);
    }
}
