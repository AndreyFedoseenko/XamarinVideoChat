using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceSpecificAppServerService.DataObjects
{
    public class Message
    {
        public int Id { get; set; }

        public int ChatId { get; set; }

        public int SenderId { get; set; }

        public User Sender { get; set; }

        public Chat Chat { get; set; }

        public string Text { get; set; }
    }
}