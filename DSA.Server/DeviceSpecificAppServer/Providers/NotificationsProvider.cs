using DeviceSpecificAppServerService;
using DeviseSpecificAppServer.Interfaces;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DeviseSpecificAppServer.Providers
{
    public class NotificationsProvider : INotificationsProvider
    {
        private NotificationHubClient hub;

        public NotificationsProvider()
        {
            MobileAppSettingsDictionary settings = Startup.Config.GetMobileAppSettingsProvider().GetMobileAppSettings();
            var notificationHubName = settings.NotificationHubName;
            var notificationHubConnection = settings.Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnection, notificationHubName);
        }

        public bool AddTagToRegistration(string Id, string tag)
        {
            try
            {
                // Return the installation for the specific ID.
                var installation = hub.GetInstallation(Id);
                hub.PatchInstallation(Id, new[]
                {
                    new PartialUpdateOperation
                    {
                        Operation = UpdateOperationType.Add,
                        Path = "/tags",
                        Value = tag
                    }
                });
                return true;
            }
            catch (MessagingException ex)
            {
                Debugger.Break();
            }
            return false;
        }

        public async Task SendNotification(Dictionary<string, string> templateParams, string tag)
        {
            try
            {
                var result = await hub.SendTemplateNotificationAsync(templateParams, tag);
            }
            catch (Exception ex)
            {
            }
        }
    }
}