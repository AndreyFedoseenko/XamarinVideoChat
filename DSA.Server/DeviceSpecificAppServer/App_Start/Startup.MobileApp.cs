using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using DeviceSpecificAppServerService.DataObjects;
using Owin;
using DeviceSpecificAppServerService.Context;

namespace DeviceSpecificAppServerService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app, HttpConfiguration config)
        {
            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .MapApiControllers()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new DeviceSpecificAppServerInitializer());

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            app.UseWebApi(config);
        }
    }

    public class DeviceSpecificAppServerInitializer : DropCreateDatabaseIfModelChanges<DeviceSpecificAppServerContext>
    {
        protected override void Seed(DeviceSpecificAppServerContext context)
        {

            //List<Message> messages = new List<Message>
            //{
            //    new Message { Sender= "Vasya", Receiver = "Petya", ChatName="Dimin Dr"},
            //    new Message { Sender= "Vitya", Receiver = "Katya", ChatName="Svadba Petrovicha"}
            //};

            //foreach (Message message in messages)
            //{
            //    context.Set<Message>().Add(message);
            //}

            List<User> users = new List<User>
            {
                new User { Id = 1, Email = "Fe.andrey@nix.com", Password = "nixsolutions11_fed" },
                new User { Id = 2, Email = "Fed.andrey@nix.cos", Password = "nixsolutions11_fed" }
            };

            foreach (User user in users)
            {
                context.Set<User>().Add(user);
            }

            base.Seed(context);
        }
    }
}

