using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AngJSWeb.Startup))]
namespace AngJSWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
