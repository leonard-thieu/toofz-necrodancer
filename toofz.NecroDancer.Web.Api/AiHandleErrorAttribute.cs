using System;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace toofz.NecroDancer.Web.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AiHandleErrorAttribute : HandleErrorAttribute
    {
        public AiHandleErrorAttribute(TelemetryClient ai)
        {
            this.ai = ai;
        }

        private readonly TelemetryClient ai;

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                // If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {   // or reuse instance (recommended!). see note above  
                    ai.TrackException(filterContext.Exception);
                }
            }
            base.OnException(filterContext);
        }
    }
}