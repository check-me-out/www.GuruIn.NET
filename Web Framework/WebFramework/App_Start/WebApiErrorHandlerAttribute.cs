using log4net;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;

namespace WebFramework.App_Start
{
    public class WebApiErrorHandlerAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _log;

        public WebApiErrorHandlerAttribute(ILog log)
        {
            _log = log;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            _log.Error("WebApiErrorHandlerAttribute", context.Exception);

#if DEBUG
            var response = new StringBuilder();
            response.AppendLine("Exception: ");
            response.Append(context.Exception.Message);

            if (context.Exception.InnerException != null)
            {
                response.AppendLine();
                response.AppendLine("Inner Exception: ");
                response.Append(context.Exception.InnerException.Message);

                if (context.Exception.InnerException.InnerException != null)
                {
                    response.AppendLine();
                    response.AppendLine("Second-level Inner Exception: ");
                    response.Append(context.Exception.InnerException.InnerException.Message);
                }
            }

            response.AppendLine();
            response.AppendLine("Stack Trace: ");
            var stackTrace = context.Exception.StackTrace.Split(new[] { " at " }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 1; i < stackTrace.Count(); i++ )
            {
                response.Append(stackTrace[i]);
            }
            response.AppendLine();
            response.AppendLine("Time: " + DateTime.Now);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent(response.ToString()),
                                ReasonPhrase = "Internal Server Error"
                            });
#else
            var response = new StringBuilder();
            response.AppendLine("Sorry. An unexpected error occured while trying to process your request.");
            response.AppendLine("The error has been logged for resolution, please try again later.");
            response.AppendLine("Time: " + DateTime.Now);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent(response.ToString()),
                                ReasonPhrase = "Internal Server Error"
                            });
#endif
        }
    }
}