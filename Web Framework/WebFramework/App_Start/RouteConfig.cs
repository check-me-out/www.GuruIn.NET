using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebFramework
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebFramework.Controllers" }
            );

            routes.MapRoute(
                "404-PageNotFound",
                "{*url}",
                new { controller = "Error", action = "PageNotFound" },
                namespaces: new[] { "WebFramework.Controllers" }
            );
        }
    }
}
