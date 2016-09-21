using log4net;
using System.Web;
using System.Web.Mvc;

namespace WebFramework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters, ILog log)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new MvcErrorHandlerAttribute(log));
        }
    }
}
