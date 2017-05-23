using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DeviceSpecificAppServerService.Castle;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(DeviceSpecificAppServerService.Startup))]

namespace DeviceSpecificAppServerService
{
    public partial class Startup
    {
        private static IWindsorContainer _container;

        public static HttpConfiguration Config { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            Config = new HttpConfiguration();
            Config.EnableSystemDiagnosticsTracing();
            ConfigureWindsor(Config);

            // Map routes by attribute
            Config.MapHttpAttributeRoutes();
            ConfigureMobileApp(app, Config);
        }

        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;
        }
    }
}