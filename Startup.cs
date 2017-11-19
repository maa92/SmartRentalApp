using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartRentalApp.Startup))]
namespace SmartRentalApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
