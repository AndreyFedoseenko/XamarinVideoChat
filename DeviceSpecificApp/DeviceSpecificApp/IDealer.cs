using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSpecificApp
{
    public interface IDealer
    {
        Task Register(string email, string password);

        Task SendMessage(string message);

        bool IsAuthentificated();

        void handleAddingMessages(Action<List<MessageContent>> handler);

        Task DisplayChatMessage();

        bool Call(string number);

        void StardRecordVideo();

        Task ConnectToSession();

        Task VideoCallSomeone();

        void RejectVideoCall();
    }
}
