using Microsoft.Azure.Mobile.Server;

namespace DeviceSpecificAppServerService.Models
{
    public class Invitation
    {
        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string ChatName { get; set; }
    }
}