using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DeviseSpecificAppServer.Interfaces;
using DeviseSpecificAppServer.Providers;
using DeviceSpecificAppServerService.Context;
using System.Reflection;
using System.Web.Http;

namespace DeviceSpecificAppServerService.Castle
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
              Component.For<INotificationsProvider>().ImplementedBy<NotificationsProvider>()
            );
            container.Register(Component.For<DeviceSpecificAppServerContext>());

            var contollers = Assembly.GetExecutingAssembly().GetTypes()
                 .Where(x => x.BaseType == typeof(ApiController)).ToList();
            foreach (var controller in contollers)
            {
                container.Register(Component.For(controller).LifestylePerWebRequest());
            }
        }
    }
}