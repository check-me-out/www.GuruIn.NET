namespace WebFramework.App_Start
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BypassAntiForgeryTokenAttribute : ActionFilterAttribute
    {
    }
}