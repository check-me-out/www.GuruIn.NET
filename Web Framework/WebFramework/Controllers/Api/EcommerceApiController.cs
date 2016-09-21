namespace WebFramework.Controllers.Api
{
    using log4net;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using WebFramework.Helpers;
    using WebFramework.Persistence.Products;
    using WebFramework.ViewModels;

    //[RoutePrefix("api/Ecommerce")]
    public class EcommerceController : ApiController
    {
        private readonly ILog _log;

        private readonly IProductsDbContext _db;

        public EcommerceController([Named(Constants.ServerLoggerName)] ILog log, IProductsDbContext db)
        {
            _log = log;
            _db = db;
        }

        public static string GetEndpointUrl(HttpRequestBase request, string routeName)
        {
            if (request == null)
            {
                return null;
            }

            var endpoint = new System.Web.Mvc.UrlHelper(request.RequestContext).RouteUrl(routeName, new { httproute = true });
            if (string.IsNullOrWhiteSpace(endpoint)) endpoint = "/api/Ecommerce";
            return endpoint;
        }

        public static string GetBaseUrl()
        {
            return "/api/Ecommerce";
        }

        public IEnumerable<Models.Products.Item> GetAllProducts()
        {
            var list = _db.Items.ToList();
            return list;
        }
    }
}