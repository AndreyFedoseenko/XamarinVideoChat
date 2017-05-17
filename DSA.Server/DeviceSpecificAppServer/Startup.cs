using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DeviceSpecificAppServerService.Startup))]

namespace DeviceSpecificAppServerService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}