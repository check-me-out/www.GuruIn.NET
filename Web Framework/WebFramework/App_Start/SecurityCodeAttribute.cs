using System.Web.Mvc;
namespace WebFramework.App_Start
{
    public class SecurityCodeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext != null && !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var requestUrl = filterContext.HttpContext.Request.Url.PathAndQuery;
                if (!requestUrl.StartsWith("/")) requestUrl = "/" + requestUrl;
                filterContext.HttpContext.Response.Redirect("/Security/Login?returnUrl=" + requestUrl, true);
            }
        }
    }
}