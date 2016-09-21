using System.Web.Http;
using System.Web.Mvc;

namespace WebFramework.Areas.Blog
{
    public class BlogAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Blog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.Routes.MapHttpRoute(
                name: "Blog_ApiByActionAndId",
                routeTemplate: "Blog/api/{action}/{slug}",
                defaults: new { controller = "Blog", slug = RouteParameter.Optional }
            );

            // IDs
            context.Routes.MapHttpRoute(
                name: "Blog_ApiById",
                routeTemplate: "Blog/api/{action}/{id}/{slug}",
                defaults: new { controller = "Blog", slug = RouteParameter.Optional },
                constraints: new { id = @"^\d+$" }
            );

            context.MapRoute(
                "Blog_default",
                "Blog/{action}/{id}/{slug}",
                new { controller = "Home", id = UrlParameter.Optional, slug = UrlParameter.Optional },
                namespaces: new[] { "WebFramework.Areas.Blog.Controllers" }
            );
        }
    }
}