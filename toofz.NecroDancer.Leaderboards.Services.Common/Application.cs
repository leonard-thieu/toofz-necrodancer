using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using log4net;
using Microsoft.ApplicationInsights;

namespace toofz.NecroDancer.Leaderboards.Services.Common
{
    public static class Application
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Application));

        public static readonly TelemetryClient TelemetryClient = InitializeTelemetry();

        static TelemetryClient InitializeTelemetry()
        {
            var telemetryClient = new TelemetryClient();

            var context = telemetryClient.Context;
            context.User.Id = Environment.UserName;
            context.Session.Id = Guid.NewGuid().ToString();
            context.Device.OperatingSystem = Environment.OSVersion.ToString();

            return telemetryClient;
        }

        public static void Run<T, TSettings>()
            where T : WorkerRoleBase<TSettings>, new()
            where TSettings : Settings, new()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Log.Error("Terminating application due to unhandled exception.", (Exception)e.ExceptionObject);
            };
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Log.Error("Terminating application due to unobserved task exception.", e.Exception);
            };

            // Start as console application
            if (Environment.UserInteractive)
            {
                using (var worker = new T())
                {
                    worker.ConsoleStart();
                }
            }

            // Start as Windows service
            else
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                ServiceBase.Run(new T());
            }
        }
    }
}
