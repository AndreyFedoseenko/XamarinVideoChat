using DeviceSpecificApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSpecificApp
{
    public interface IDealer
    {
        Task<bool> Register(string email, string password);

        string GetRegistrationId();

        Task SendMessage(string message);

        bool IsAuthentificated();

        void handleAddingMessages(Action<List<MessageContent>> handler);

        Task DisplayChatMessage();

        bool Call(string number);

        //void StardRecordVideo();

        Task ConnectToSession();

        Task VideoCallSomeone();

        void RejectVideoCall();

        void SetUpMessageHandler(Action handler);
    }
}
