using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Mobile.Server;

namespace DeviceSpecificAppServerService.DataObjects
{
    public class User
    {
        public User()
        {
            Chats = new List<Chat>();
            Messages = new List<Message>();
        }
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<Chat> Chats { get; set; }

        public List<Message> Messages { get; set; }
    }
}