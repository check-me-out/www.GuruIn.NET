using WebFramework.App_Start;
using log4net;
using System.Web.Mvc;

namespace WebFramework
{
    public class MvcErrorHandlerAttribute : HandleErrorAttribute
    {
        private readonly ILog _log;

        public MvcErrorHandlerAttribute(ILog log)
        {
            _log = log;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            _log.Error("MvcErrorHandlerAttribute", filterContext.Exception);

            // if the request is AJAX return JSON else view.
            if (IsAjax(filterContext))
            {
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else
            {
                //Normal Exception
                base.OnException(filterContext);

                filterContext.ExceptionHandled = true;
            }

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];

//#if DEBUG
            filterContext.Controller.TempData["IsDebugMode"] = true;
#if DEBUG
            filterContext.Controller.TempData["ShowExceptionDetails"] = true;
#endif

            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            var viewData = new ViewDataDictionary<HandleErrorInfo>(model);

            filterContext.Result = new ViewResult 
                                            { 
                                                ViewName = this.View, 
                                                MasterName = this.Master, 
                                                ViewData = viewData, 
                                                TempData = filterContext.Controller.TempData 
                                            };
        }

        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}