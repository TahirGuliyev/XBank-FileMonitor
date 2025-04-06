using Serilog;
using System.Configuration;
using System.Data;
using System.Windows;

namespace XBank.FileMonitor.App
{
    public partial class App : Application
    {
        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Application started.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Application exiting.");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }
}
