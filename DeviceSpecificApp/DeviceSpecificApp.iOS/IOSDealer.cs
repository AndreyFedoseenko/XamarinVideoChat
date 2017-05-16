using System.Linq;
using Xamarin.Forms;
using DeviceSpecificApp;
using System;
using Firebase.Auth;
using Firebase.Xamarin.Database;
using System.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;
using DeviceSpecificApp.iOS;
using Firebase.Xamarin.Auth;
using Foundation;
using UIKit;

[assembly: Dependency(typeof(IOSDealer))]
namespace DeviceSpecificApp.iOS
{
    public class IOSDealer : IDealer
    {
        Auth auth;
        FirebaseClient firebase;
        Action<List<MessageContent>> handler;

        public IOSDealer()
        {
            firebase = new FirebaseClient("https://simplemessager.firebaseio.com/");
            try
            {
                Database.DefaultInstance.GetRootReference().GetChild("chats").ObserveEvent(DataEventType.Value, async (snapshot) =>
                {
                    var folderData = snapshot.GetValue<NSDictionary>();
                    // Do magic with the folder data
                    await DisplayChatMessage();
                });
            }
            catch (Exception ex)
            {
                var z = ex.Message;
            }
            // auth = Auth.DefaultInstance;
        }

        public bool Call(string number)
        {
            return UIApplication.SharedApplication.OpenUrl(
                new NSUrl("tel:" + number));
        }

        public async Task DisplayChatMessage()
        {
            try
            {
                var items = await firebase.Child("chats")
                    .OnceAsync<MessageContent>();

                if (items.Count > 0 && handler != null)
                {
                    handler(items.Select(x => new MessageContent
                    {
                        Email = x.Object.Email,
                        Message = x.Object.Message,
                        Phone = x.Object.Phone
                    }).ToList());
                }
            }
            catch (Exception ex)
            {
                var z = ex.Message;
            }
        }

        public void handleAddingMessages(Action<List<MessageContent>> handler)
        {
            this.handler = handler;
        }

        public bool IsAuthentificated()
        {
            return Auth.DefaultInstance.CurrentUser != null;
        }

        public async Task Register(string email, string password)
        {
            try
            {
                Auth.DefaultInstance.SignIn(email, password, (user, error) =>
                {
                    var usr = Auth.DefaultInstance.CurrentUser;
                });
            }
            catch (Exception ex)
            {
                var z = ex.Message;
            }
        }

        public async Task SendMessage(string message)
        {
            var items = await firebase.Child("chats").PostAsync(new MessageContent
            {
                Email = (Auth.DefaultInstance.CurrentUser != null) ? Auth.DefaultInstance.CurrentUser.Email : "iosuser@gmail.com",
                Message = message
            });

        }

        //public void StardRecordVideo()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task VideoCallSomeone()
        {
            AppDelegate.SelfDelegate.VideoProvider.DoPublish();
        }

        public void RejectVideoCall()
        {
            AppDelegate.SelfDelegate.VideoProvider.DoDisconnect();
        }

        public async Task ConnectToSession()
        {
            AppDelegate.SelfDelegate.VideoProvider.DoConnect();
        }
    }
}