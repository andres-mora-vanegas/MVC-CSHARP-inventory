using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(testi2.Startup))]
namespace testi2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
