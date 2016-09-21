namespace WebFramework.Controllers
{
    using log4net;
    using Ninject;
    using System;
    using System.Web.Mvc;
    using WebFramework.Helpers;
    using WebFramework.ViewModels;

    public class HomeController : BaseController
    {
        private readonly ILog _log;

        public HomeController([Named(Constants.ServerLoggerName)] ILog log)
            : base(log)
        {
            _log = log;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var title = "Message for GuruIn.NET";
                    var subject = string.Format("Message from {0} for GuruIn.NET", model.Name);

                    var template = EmailEngine.GetTemplate("contact-us-template");
                    var body = template.Replace("*|MC:TITLE|*", title)
                                       .Replace("*|MC:SUBJECT|*", subject)
                                       .Replace("*|MC:EMAILTOBROWSERLINK|*", "http://www.GuruIn.NET")
                                       .Replace("{0}", model.Name)
                                       .Replace("{1}", model.Email)
                                       .Replace("{2}", model.Message.Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace("\r", "<br/>"))
                                       .Replace("*|CURRENT_YEAR|*", DateTime.Now.Year.ToString())
                                       .Replace("*|LIST:COMPANY|*", "GuruIn.NET");

                    var fromTo = ConfigManager.Get<string>("SmtpUserName");

                    var result = EmailEngine.SendEmail(fromTo, "GuruIn.NET", fromTo, subject, subject, body, cc: (model.CopyUser && !string.IsNullOrWhiteSpace(model.Email) ? model.Email : null));
                    if (result)
                    {
                        Session["USER_MESSAGE"] = "Your message has been successfully sent. Thank you.";
                        Session["USER_MESSAGE_SEVERITY"] = "Success";
                        Session["USER_MESSAGE_TITLE"] = "Message Sent";

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    Session["USER_MESSAGE"] = "Sorry, there was an error sending the message. Please try again later.";
                    Session["USER_MESSAGE_SEVERITY"] = "Error";

                    _log.Error("HomeController - Contact", ex);
                }
            }

            return View(model);
        }
    }
}