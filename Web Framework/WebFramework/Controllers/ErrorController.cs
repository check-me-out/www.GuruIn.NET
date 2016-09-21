using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFramework.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            // Hack for GoDaddy Hosting
            Response.TrySkipIisCustomErrors = true;

            Response.StatusCode = 500;

            var ex = RetrieveLastException();

#if DEBUG
            TempData["IsDebugMode"] = true;
            TempData["ShowExceptionDetails"] = true;
#endif

            return View(new HandleErrorInfo(ex ?? new Exception(), "UNKNOWN", "UNKNOWN"));
        }

        public ActionResult PageNotFound()
        {
            // Hack for GoDaddy Hosting
            Response.TrySkipIisCustomErrors = true;

            Response.StatusCode = 404;

            var ex = RetrieveLastException();

#if DEBUG
            TempData["IsDebugMode"] = true;
            TempData["ShowExceptionDetails"] = true;
#endif

            return View(new HandleErrorInfo(ex ?? new Exception(), "UNKNOWN", "UNKNOWN"));
        }

        public JsonResult AjaxResponse()
        {
            // Hack for GoDaddy Hosting
            Response.TrySkipIisCustomErrors = true;

            Response.StatusCode = 500;

            var errorMessage = (string)Session["JSON_ERROR_MESSAGE"];
            Session["JSON_ERROR_MESSAGE"] = null;

            return new JsonResult() { Data = errorMessage, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private Exception RetrieveLastException()
        {
            var ex = Server.GetLastError();
            if (ex == null)
            {
                //var exceptionMessage = Request.Headers.Get("EXCEPTION_MESSAGE");
                var exceptionMessage = (string)Session["EXCEPTION_MESSAGE"];
                if (string.IsNullOrWhiteSpace(exceptionMessage))
                {
                    return null;
                    //exceptionMessage = "Sorry, an unexpected error occured while trying to process your request. Please try again later.";
                }

                Exception innerException = null;
                var innerExceptionMessage = (string)Session["INNER_EXCEPTION_MESSAGE"];
                if (!string.IsNullOrWhiteSpace(innerExceptionMessage))
                {
                    Exception innerInnerException = null;
                    var innerInnerExceptionMessage = (string)Session["INNER_INNER_EXCEPTION_MESSAGE"];
                    if (!string.IsNullOrWhiteSpace(innerInnerExceptionMessage))
                    {
                        innerInnerException = new Exception(innerInnerExceptionMessage);
                        innerException = new Exception(innerExceptionMessage, innerInnerException);
                        ex = new Exception(exceptionMessage, innerException);
                    }
                    else
                    {
                        innerException = new Exception(innerExceptionMessage);
                        ex = new Exception(exceptionMessage, innerException);
                    }
                }
                else
                {
                    ex = new Exception(exceptionMessage);
                }

                var stackTrace = (string)Session["STACK_TRACE"];
                ex.Data["stackTrace"] = stackTrace;
            }

            Server.ClearError();
            Session["EXCEPTION_MESSAGE"] = null;
            Session["INNER_EXCEPTION_MESSAGE"] = null;
            Session["INNER_INNER_EXCEPTION_MESSAGE"] = null;
            Session["STACK_TRACE"] = null;

            return ex;
        }
    }
}
