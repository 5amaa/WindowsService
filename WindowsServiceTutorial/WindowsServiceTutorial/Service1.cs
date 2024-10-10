using Serilog;
using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using WindowsServiceTutorial.SMS.Logging;
using WindowsServiceTutorial.SMS.SmsService;

namespace WindowsServiceTutorial
{
    public partial class Service1 : ServiceBase
    {
        private readonly ISmsService _smsService;
        private readonly ILogService _logService;
        private Timer _timer;      // SMS sending timer
        private bool _isProcessing = false;
        private Timer _dailyTimer; // Daily midnight log check timer

        public Service1(ISmsService smsService, ILogService logService)
        {
            InitializeComponent();
            _smsService = smsService;
            _logService = logService;

            // Timer to check and send pending SMS every 2 minutes
            _timer = new Timer(120000); // 2 minutes interval
            _timer.Elapsed += async (sender, e) => await OnTimerElapsedAsync();
            _timer.AutoReset = true;

            // Set up the daily task at 12 AM
            ScheduleDailyTask();
        }

        protected override void OnStart(string[] args)
        {
            Log.Information("SMS Windows Service has started.");

            // Start the SMS sending timer
            _timer.Start();

            // Optionally trigger the SMS sending immediately on startup
            OnTimerElapsedAsync();
        }

        protected override void OnStop()
        {
            Log.Information("SMS Windows Service has stopped.");

            // Stop both timers
            _timer.Stop();
            _dailyTimer.Stop();
        }

        private async Task OnTimerElapsedAsync()
        {
            if (_isProcessing) // Skip if already processing
            {
                Log.Information("SMS sending is still in progress, skipping this cycle.");
                return;
            }

            _isProcessing = true; // Set flag to true to prevent overlapping tasks
            try
            {
                Log.Information("Checking and sending pending SMS...");
                await _smsService.CheckAndSendPendingSms(); // The method to send SMS
            }
            catch (Exception ex)
            {
                Log.Error($"Error in SMS sending: {ex}");
            }
            finally
            {
                _isProcessing = false; // Reset flag after processing
            }
        }

        private void ScheduleDailyTask()
        {
            DateTime now = DateTime.Now;
            DateTime nextMidnight = DateTime.Today.AddDays(1); // Next midnight

            TimeSpan timeUntilMidnight = nextMidnight - now;

            // Set the timer to trigger at midnight
            _dailyTimer = new Timer(timeUntilMidnight.TotalMilliseconds);
            _dailyTimer.Elapsed += async (sender, e) => await OnDailyLogCheckAsync();
            _dailyTimer.AutoReset = false; // Trigger only once initially

            // Reset the timer to 24-hour intervals after the first run
            _dailyTimer.Start();
        }

        private async Task OnDailyLogCheckAsync()
        {
            try
            {
                Log.Information("Running daily log check at midnight...");
                await _logService.RunDailyCheckAsync(); // Log check at midnight
            }
            catch (Exception ex)
            {
                Log.Error($"Error during daily log check: {ex}");
            }

            // Reset the timer to trigger every 24 hours after this run
            ResetDailyTimer();
        }

        private void ResetDailyTimer()
        {
            // Set the interval to 24 hours for future executions
            _dailyTimer.Interval = TimeSpan.FromHours(24).TotalMilliseconds;
            _dailyTimer.AutoReset = true; // Repeat every 24 hours
            _dailyTimer.Start();
        }
    }
}
