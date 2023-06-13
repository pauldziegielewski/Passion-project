using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Passion_project.Startup))]
namespace Passion_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
