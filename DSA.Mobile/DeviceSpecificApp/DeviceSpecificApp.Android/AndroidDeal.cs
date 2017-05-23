using Android.Content;
using Android.Telephony;
using System.Linq;
using Xamarin.Forms;
using DeviceSpecificApp;
using Uri = Android.Net.Uri;
using DeviceSpecificApp.Droid;
using System;
using Firebase.Auth;
using Android.Gms.Tasks;
using Android.Widget;
using Firebase.Xamarin.Database;
using System.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;
using Android.Views;
using Android.Views.InputMethods;
using Android.App;
using Android.Media;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using DeviceSpecificApp.Model;

[assembly: Dependency(typeof(AndroidDeal))]
namespace DeviceSpecificApp.Droid
{
    public class AndroidDeal : IDealer
    {
        FirebaseAuth auth;
        FirebaseClient firebase;
        Action<List<MessageContent>> handler;
        public static Action AcceptInvitationMessageHandler { get; private set; }

        public AndroidDeal()
        {
            firebase = new FirebaseClient("https://simplemessager.firebaseio.com/");
            try
            {
                MainActivity.CurrentActivity.firebase = firebase;
                FirebaseDatabase.Instance.GetReference("chats").AddValueEventListener(MainActivity.CurrentActivity);
            }
            catch(Exception ex)
            {
                var z = ex.Message;
            }
            auth = FirebaseAuth.Instance;
        }

        public void handleAddingMessages(Action<List<MessageContent>> handler)
        {
            MainActivity.CurrentActivity.handler = handler;
        }

        public bool IsAuthentificated()
        {
            return FirebaseAuth.Instance.CurrentUser != null;
        }

        public async System.Threading.Tasks.Task<bool> Register(string email, string password)
        {
            try
            {
                //await auth.SignInWithEmailAndPasswordAsync(email, password);
                //var user = await MainActivity.CurrentActivity.MobileClient.LoginAsync(MainActivity.CurrentActivity, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                //var user = await MainActivity.CurrentActivity.MobileClient.LoginAsync(MainActivity.CurrentActivity, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                //return FirebaseAuth.Instance.CurrentUser != null;
                //GcmService.RegisterTagOnServer(email);
            }
            catch(Exception ex)
            {
            }
            return false;
        }

        public async System.Threading.Tasks.Task SendMessage(string message)
        {
            var items = await firebase.Child("chats").PostAsync(new MessageContent{
                Email = FirebaseAuth.Instance.CurrentUser.Email,
                Message = message
            });

        }

        public async System.Threading.Tasks.Task DisplayChatMessage()
        {
            await MainActivity.CurrentActivity.DisplayChatMessage();
        }

        public bool Call(string number)
        {
            var context = Forms.Context;
            if (context == null)
                return false;

            var intent = new Intent(Intent.ActionCall);
            intent.SetData(Uri.Parse("tel:" + number));

            if (IsIntentAvailable(context, intent))
            {
                context.StartActivity(intent);
                return true;
            }

            return false;

        }

        private static bool IsIntentAvailable(Context context, Intent intent)
        {
            var packageManager = context.PackageManager;

            var list = packageManager.QueryIntentServices(intent, 0)
                .Union(packageManager.QueryIntentActivities(intent, 0));

            if (list.Any())
                return true;

            var manager = TelephonyManager.FromContext(context);
            return manager.PhoneType != PhoneType.None;
        }

        public async System.Threading.Tasks.Task VideoCallSomeone()
        {
            await MainActivity.CurrentActivity.VideoCallSomeOne();
        }

        public void RejectVideoCall()
        {
            MainActivity.CurrentActivity.RejectVideoCall();
        }

        public async System.Threading.Tasks.Task ConnectToSession()
        {
            await MainActivity.CurrentActivity.ConnectToSession();
        }

        public void SetUpMessageHandler(Action handler)
        {
            AcceptInvitationMessageHandler = handler;
        }

        public string GetRegistrationId()
        {
            return GcmService.RegistrationID;
        }
    }
}