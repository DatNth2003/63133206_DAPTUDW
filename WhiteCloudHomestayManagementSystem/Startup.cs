using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WhiteCloudHomestayManagementSystem.Startup))]
namespace WhiteCloudHomestayManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
