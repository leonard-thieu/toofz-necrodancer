using System;
using log4net;
using Microsoft.ApplicationInsights.Extensibility;
using toofz.NecroDancer.Leaderboards.Services.Common;

namespace toofz.NecroDancer.Leaderboards.Services.PlayerUpdate
{
    static class Program
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            Log.Debug("Initialized logging.");
            var instrumentationKey = Environment.GetEnvironmentVariable("PlayersInstrumentationKey", EnvironmentVariableTarget.Machine);
            if (instrumentationKey != null) { TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey; }
            else
            {
                Log.Warn($"The environment variable 'PlayersInstrumentationKey' is not set. Telemetry is disabled.");
                TelemetryConfiguration.Active.DisableTelemetry = true;
            }
            Application.Run<WorkerRole, PlayerSettings>();
        }
    }
}
