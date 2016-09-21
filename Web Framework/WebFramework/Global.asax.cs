using WebFramework.App_Start;
using log4net;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Configuration;
using WebFramework.Helpers;
using WebFramework.Controllers;
using System.IO;
using Newtonsoft.Json;

namespace WebFramework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            // lazy load DI.
            var logger = ModuleLoader.DI.Get<ILog>(Constants.ServerLoggerName);

            var diResolver = new DiResolver(ModuleLoader.DI);
            DependencyResolver.SetResolver(diResolver); // MVC
            GlobalConfiguration.Configuration.DependencyResolver = diResolver; // WebAPI

            // To resolve self referencing loop error.
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Re‌ferenceLoopHandling = ReferenceLoopHandling.Ignore;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration, logger);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, logger);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.EnsureInitialized();

            EmailEngine.Logger(logger);
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();

            try
            {
                var logger = ModuleLoader.DI.Get<ILog>(Constants.ServerLoggerName);
                logger.Error("Application_Error", ex);
            }
            catch (Exception exc)
            {
                exc = exc.InnerException;
            }

            var code = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500;

            Server.ClearError();
            Response.Clear();
            Response.ContentType = "text/html";
            Response.StatusCode = code;

            // Hack for GoDaddy Hosting
            Response.TrySkipIisCustomErrors = true;

            // if the request is AJAX return JSON else view.
            if (IsAjax())
            {
                var errorMsg = (ex != null ? ex.Message : string.Empty);
                errorMsg += (ex != null && ex.InnerException != null ? ex.InnerException.Message : string.Empty);
                errorMsg += (ex != null && ex.InnerException != null && ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : string.Empty);
                Context.Session["JSON_ERROR_MESSAGE"] = errorMsg;

                Context.Response.Redirect("~/Error/AjaxResponse", false);
                return;
            }

            var model = new HandleErrorInfo(ex, "UNKNOWN", "UNKNOWN");
            if (code == 404)
            {
                var result = RenderRazorViewToString("~/Views/Error/PageNotFound.cshtml", "~/Views/Shared/_commonLayout.cshtml", model);

                Response.Write(result);
            }
            else
            {
                var result = RenderRazorViewToString("~/Views/Error/Index.cshtml", "~/Views/Shared/_commonLayout.cshtml", model);

                Response.Write(result);
            }
        }

        public string RenderRazorViewToString(string viewName, string masterName, object model)
        {
            var controller = CreateController<ErrorController>();
            var context = controller.ControllerContext;

            var viewData = new ViewDataDictionary(model);
            var tempData = new TempDataDictionary();

            tempData["VersionInfo"] = BaseController.GetVersionInfo();

#if DEBUG
            tempData["IsDebugMode"] = true;
            tempData["ShowExceptionDetails"] = true;
#endif

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(context, viewName, masterName);
                var viewContext = new ViewContext(context, viewResult.View,
                                             viewData, tempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(context, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }

        private bool IsAjax()
        {
            return Context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private static T CreateController<T>(RouteData routeData = null)
                    where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper = null;
            if (HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }
    }
}
