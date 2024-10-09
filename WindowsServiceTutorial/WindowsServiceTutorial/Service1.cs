using System;
using System.IO;
using System.ServiceProcess;

namespace WindowsServiceTutorial
{
    public partial class Service1 : ServiceBase
    {
        private string logFilePath = @"C:\WindowsServiceLog\log.txt"; // Specify your log file path

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log("My Windows Service has started.");
        }

        protected override void OnStop()
        {
            Log("My Windows Service has stopped.");
        }

        private void Log(string message)
        {
            // Create or append to the log file
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}
