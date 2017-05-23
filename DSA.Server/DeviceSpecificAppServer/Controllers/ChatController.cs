using DeviceSpecificAppServerService.Context;
using DeviceSpecificAppServerService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using DeviceSpecificAppServerService.DataObjects;
using DeviseSpecificAppServer.Models;
using DeviseSpecificAppServer.Interfaces;

namespace DeviceSpecificAppServerService.Controllers
{
    [RoutePrefix("api/chat")]
    public class ChatController : ApiController
    {
        private DeviceSpecificAppServerContext context;

        private INotificationsProvider notificationsProvider;

        public ChatController(DeviceSpecificAppServerContext context,
            INotificationsProvider notificationsProvider)
        {
            this.context = context;
            this.notificationsProvider = notificationsProvider;
        }

        [HttpGet, Route("chats")]
        public IHttpActionResult GetChats([FromUri] string userName)
        {
            var user = this.context.Users.Include(x => x.Chats).FirstOrDefault(x => x.Email == userName);
            var chatNames = user.Chats.Select(x => x.Name).ToList();
            return this.Json(chatNames);
        }

        [HttpGet, Route("messages")]
        public IHttpActionResult GetMessages([FromUri] UserChatModel model)
        {
            var messages = this.context.Messages
                .Include(x => x.Chat)
                .Include(x => x.Sender)
                .Where(x => x.Chat.Name == model.ChatName)
                .ToList();
            var result = messages.Select(x => new MessageInfo()
            {
                Text = x.Text,
                ChatName = x.Chat.Name,
                SenderEmail = x.Sender.Email
            }).ToList();
            return this.Json(result);
        }

        [HttpPost, Route("create")]
        public IHttpActionResult CreateChat([FromBody] UserChatModel model)
        {
            var user = this.context.Users.FirstOrDefault(x => x.Email == model.Email);
            var chat = new Chat()
            {
                Name = model.ChatName
            };
            
            this.context.Chats.Add(chat);
            user.Chats.Add(chat);
            this.context.SaveChanges();
            return this.Ok();
        }

        [HttpPost, Route("send")]
        public IHttpActionResult SendMessage([FromBody] MessageInfo model)
        {
            var sender = this.context.Users.FirstOrDefault(x => x.Email == model.SenderEmail);
            var chat = this.context.Chats.Include(x => x.Users).FirstOrDefault(x => x.Name == model.ChatName);
            this.context.Messages.Add(new Message()
            {
                ChatId = chat.Id,
                SenderId = sender.Id,
                Text = model.Text
            });
            this.context.SaveChanges();

            var reseiverEmails = chat.Users.Select(x => x.Email).ToArray();

            for (int i = 0; i < reseiverEmails.Length; i++)
            {
                string userTag = "_UserId:" + reseiverEmails[i];

                if(reseiverEmails[i] != sender.Email)
                {
                    var templateParams = new Dictionary<string, string>();
                    templateParams["senderParam"] = model.SenderEmail;
                    templateParams["chatParam"] = model.ChatName;
                    templateParams["textParam"] = model.Text;
                    templateParams["isInvitationParam"] = false.ToString();
                    this.notificationsProvider.SendNotification(templateParams, reseiverEmails[i]);
                }
            }
            return this.Ok();
        }

        [HttpPost, Route("invite")]
        public IHttpActionResult InviteUser([FromBody] Invitation model)
        {
            var templateParams = new Dictionary<string, string>();
            templateParams["senderParam"] = model.Sender;
            templateParams["chatParam"] = model.ChatName;
            templateParams["receiverParam"] = model.Receiver;
            templateParams["isInvitationParam"] = true.ToString();

            var receinver = this.context.Users.FirstOrDefault(x => x.Email == model.Receiver);
            if(receinver != null)
            {
                this.notificationsProvider.SendNotification(templateParams, receinver.Email);
                return this.Ok();
            }
            return this.BadRequest();
        }

        [HttpPost, Route("accept")]
        public IHttpActionResult AcceptInvitation([FromBody] Invitation model)
        {
            var chat = this.context.Chats.FirstOrDefault(x => x.Name == model.ChatName);
            var receiver = this.context.Users.FirstOrDefault(x => x.Email == model.Receiver);
            receiver.Chats.Add(chat);
            this.context.SaveChanges();
            return this.Ok();
        }
    }
}