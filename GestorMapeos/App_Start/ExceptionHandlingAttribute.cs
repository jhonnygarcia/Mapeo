using System;
using System.Web.Http.Filters;
using log4net;
using Newtonsoft.Json;

namespace GestorMapeos.App_Start
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var arguments = JsonConvert.SerializeObject(context.ActionContext.ActionArguments);
            var formatException = "{0} Url: {1}" + Environment.NewLine  + 
                                  "Arguments: {2}" + Environment.NewLine +
                                  "Exception: {3}" + Environment.NewLine +
                                  "StackTrace: {4}";
            var log = LogManager.GetLogger(typeof(ExceptionHandlingAttribute));

            log.Error(string.Format(formatException,
                context.Request.Method.Method,
                context.Request.RequestUri.AbsoluteUri,
                arguments,
                context.Exception.Message,
                context.Exception.StackTrace));
        }
    }
}