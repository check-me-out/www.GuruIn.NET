namespace WebFramework.Controllers
{
    using log4net;
    using Ninject;
    using System;
    using System.Web.Mvc;
    using WebFramework.Helpers;
    using WebFramework.ViewModels;

    public class DemosController : BaseController
    {
        private readonly ILog _log;

        public DemosController([Named(Constants.ServerLoggerName)] ILog log)
            : base(log)
        {
            _log = log;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}