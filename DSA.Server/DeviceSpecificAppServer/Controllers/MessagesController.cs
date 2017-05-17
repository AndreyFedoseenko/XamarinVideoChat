using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using System.Collections.Generic;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server;
using DeviceSpecificAppServerService.DataObjects;
using DeviceSpecificAppServerService.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace FirebaseXamarinAppService.Controllers
{
    public class MessagesController : TableController<Message>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DeviceSpecificAppServerContext context = new DeviceSpecificAppServerContext();
            DomainManager = new EntityDomainManager<Message>(context, Request);
        }

        // GET tables/TodoItem
        public IQueryable<Message> GetAllTodoItems()
        {
            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Message> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Message> PatchTodoItem(string id, Delta<Message> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostTodoItem(Message item)
        {
            Message current = await InsertAsync(item);

            HttpConfiguration config = this.Configuration;
            MobileAppSettingsDictionary settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            var notificationHubName = settings.NotificationHubName;
            var notificationHubConnection = settings.Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            var hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            var templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = item.Sender + " invite you to" + item.ChatName;

            try
            {
                var result = await hub.SendTemplateNotificationAsync(templateParams);

                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (Exception ex)
            {
                config.Services.GetTraceWriter().Error(ex.Message, null, "Push.SendAsync Error");
            }

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}