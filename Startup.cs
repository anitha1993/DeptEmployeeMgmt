using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeptEmpMgmt.Startup))]
namespace DeptEmpMgmt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
