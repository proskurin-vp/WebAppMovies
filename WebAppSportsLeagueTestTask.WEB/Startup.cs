using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppSportsLeagueTestTask.WEB.Startup))]
namespace WebAppSportsLeagueTestTask.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
