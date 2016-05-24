using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Spotify.Startup))]
namespace Spotify
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
