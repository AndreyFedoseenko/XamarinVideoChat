using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceSpecificApp.Model
{
    public class MessageInfo
    {
        public string SenderEmail { get; set; }

        public string Text { get; set; }

        public string ChatName { get; set; }

        public string Phone { get; set; }

        public string Time { get; set; }

        public MessageInfo()
        {
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Phone = "0930438556";
        }
    }
}