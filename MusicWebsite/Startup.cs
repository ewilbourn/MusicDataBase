using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicWebsite.Startup))]
namespace MusicWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
