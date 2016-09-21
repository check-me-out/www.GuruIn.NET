using System.Web.Mvc;

namespace WebFramework.ViewModels
{
    public class ModalViewModel
    {
        public MvcHtmlString Title { get; set; }
        public MvcHtmlString Body { get; set; }
        public MvcHtmlString Cancel { get; set; }
        public MvcHtmlString Ok { get; set; }
    }
}