namespace WebFramework.Controllers
{
    using System.Web.Mvc;

    public class FiddleController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
