namespace WebFramework.Controllers
{
    using log4net;
    using Ninject;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Web.Mvc;
    using WebFramework.App_Start;
    using WebFramework.Helpers;

    [AntiForgeryTokenOnPost]
    public class BaseController : Controller
    {
        private readonly ILog _log;

        public BaseController()
        {
            _log = ModuleLoader.DI.Get<ILog>(Constants.ServerLoggerName);
        }

        public BaseController(ILog log)
        {
            _log = log;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            TempData["UserMessage"] = (string)Session["USER_MESSAGE"];
            TempData["UserMessageSeverity"] = (string)Session["USER_MESSAGE_SEVERITY"];
            TempData["UserMessageTitle"] = (string)Session["USER_MESSAGE_TITLE"];

            TempData["VersionInfo"] = GetVersionInfo();

//#if DEBUG
            TempData["IsDebugMode"] = true;
//#endif
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.Response.IsRequestBeingRedirected)
            {
                Session["USER_MESSAGE"] = null;
                Session["USER_MESSAGE_SEVERITY"] = null;
                Session["USER_MESSAGE_TITLE"] = null;
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            _log.Error("BaseController.OnException", filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();

            // if the request is AJAX return JSON else view.
            if (IsAjax(filterContext))
            {
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];

            filterContext.Controller.TempData["VersionInfo"] = GetVersionInfo();

//#if DEBUG
            filterContext.Controller.TempData["IsDebugMode"] = true;
#if DEBUG
            filterContext.Controller.TempData["ShowExceptionDetails"] = true;
#endif

            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            var viewData = new ViewDataDictionary<HandleErrorInfo>(model);

            filterContext.Result = new ViewResult 
                                            {
                                                ViewName = "~/Views/Error/Index.cshtml",
                                                MasterName = "~/Views/Shared/_commonLayout.cshtml", 
                                                ViewData = viewData, 
                                                TempData = filterContext.Controller.TempData 
                                            };
        }

        public static string GetVersionInfo()
        {
            var version = string.Empty;
            try
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception)
            {
                version = "?.?.?.?";
            }

            return version;
        }

        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}