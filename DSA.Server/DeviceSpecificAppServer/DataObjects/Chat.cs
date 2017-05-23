using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceSpecificAppServerService.DataObjects
{
    public class Chat
    {
        public Chat()
        {
            Users = new List<User>();
            Messages = new List<Message>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }

        public List<Message> Messages { get; set; }
    }
}