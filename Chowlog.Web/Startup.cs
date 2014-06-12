using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chowlog.Web.Startup))]
namespace Chowlog.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
