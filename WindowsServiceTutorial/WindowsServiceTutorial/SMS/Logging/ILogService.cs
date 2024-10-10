using System.Threading.Tasks;

namespace WindowsServiceTutorial.SMS.Logging
{
    public interface ILogService
    {
        Task RunDailyCheckAsync();
    }
}
