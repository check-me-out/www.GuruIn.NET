using WebFramework.App_Start;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebFramework
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, ILog log)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ApiByActionAndId",
                routeTemplate: "api/{controller}/{action}/{data}"
            );

            // IDs
            config.Routes.MapHttpRoute(
                name: "ApiById",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { },
                constraints: new { id = @"^\d+$" }
            );

            // GUIDs
            config.Routes.MapHttpRoute(
                name: "ApiByGuid",
                routeTemplate: "api/{controller}/{guid}",
                defaults: new { },
                constraints: new { guid = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "api/{controller}/{action}/{*data}",
                defaults: new { data = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "404-ResourceNotFound",
                routeTemplate: "api/{*data}",
                defaults: new { }
            );

            config.MessageHandlers.Add(new WebApiMessageHandler());

            config.Filters.Add(new WebApiErrorHandlerAttribute(log));
        }
    }
}
