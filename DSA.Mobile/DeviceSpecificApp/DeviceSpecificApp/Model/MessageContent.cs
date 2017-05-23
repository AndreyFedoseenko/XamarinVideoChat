using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSpecificApp.Model
{
    public class MessageContent
    {
        public string Email { get; set; }

        public string Message { get; set; }

        public string Phone { get; set; }

        public string Time { get; set; }

        public MessageContent()
        {
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Phone = "0930438556";
        }
    }
}
