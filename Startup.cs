using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SubmitBug.Startup))]
namespace SubmitBug
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
