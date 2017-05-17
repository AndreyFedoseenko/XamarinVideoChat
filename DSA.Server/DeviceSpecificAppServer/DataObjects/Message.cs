using Microsoft.Azure.Mobile.Server;

namespace DeviceSpecificAppServerService.DataObjects
{
    public class Message : EntityData
    {
        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string ChatName { get; set; }
    }
}