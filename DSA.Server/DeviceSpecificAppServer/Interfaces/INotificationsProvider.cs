using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviseSpecificAppServer.Interfaces
{
    public interface INotificationsProvider
    {
        bool AddTagToRegistration(string Id, string tag);

        Task SendNotification(Dictionary<string, string> templateParams, string tag);
    }
}
