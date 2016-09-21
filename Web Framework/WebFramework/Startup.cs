using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebFramework.Startup))]
namespace WebFramework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
