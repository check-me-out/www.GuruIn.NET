namespace WebFramework.Controllers
{
    using log4net;
    using Ninject;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using WebFramework.Helpers;
    using WebFramework.Persistence.Products;
    using WebFramework.ViewModels;

    public class EcommerceController : BaseController
    {
        private readonly ILog _log;

        public EcommerceController([Named(Constants.ServerLoggerName)] ILog log)
            : base(log)
        {
            _log = log;
        }

        public ActionResult Index()
        {
            return View((object)Api.EcommerceController.GetBaseUrl());
        }
    }
}