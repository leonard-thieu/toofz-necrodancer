using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;

namespace toofz.NecroDancer.Web.Api
{
    public class AiExceptionLogger : ExceptionLogger
    {
        private readonly TelemetryClient ai = new TelemetryClient();

        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                ai.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}