using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HospitalCMS.Startup))]
namespace HospitalCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
