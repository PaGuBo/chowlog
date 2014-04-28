using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(chowlog.Startup))]
namespace chowlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
