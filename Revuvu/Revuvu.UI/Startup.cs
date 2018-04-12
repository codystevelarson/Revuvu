using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Revuvu.UI.Startup))]
namespace Revuvu.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
