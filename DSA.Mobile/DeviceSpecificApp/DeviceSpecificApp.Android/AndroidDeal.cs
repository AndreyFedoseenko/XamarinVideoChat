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

[assembly: Dependency(typeof(AndroidDeal))]
namespace DeviceSpecificApp.Droid
{
    public class AndroidDeal : IDealer
    {
        FirebaseAuth auth;
        FirebaseClient firebase;
        Action<List<MessageContent>> handler;

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

        public async System.Threading.Tasks.Task Register(string email, string password)
        {
            try
            {
                await auth.SignInWithEmailAndPasswordAsync(email, password);
            }
            catch(Exception ex)
            {
                var z = ex.Message;
            }
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

        public static bool IsIntentAvailable(Context context, Intent intent)
        {
            var packageManager = context.PackageManager;

            var list = packageManager.QueryIntentServices(intent, 0)
                .Union(packageManager.QueryIntentActivities(intent, 0));

            if (list.Any())
                return true;

            var manager = TelephonyManager.FromContext(context);
            return manager.PhoneType != PhoneType.None;
        }

        //public void StardRecordVideo()
        //{
        //    string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/test.mp4";
        //    mainActivity.Recorder = new MediaRecorder();
        //    mainActivity.Recorder.SetVideoSource(VideoSource.Camera);
        //    mainActivity.Recorder.SetAudioSource(AudioSource.Mic);
        //    mainActivity.Recorder.SetOutputFormat(OutputFormat.Default);
        //    mainActivity.Recorder.SetVideoEncoder(VideoEncoder.Default);
        //    mainActivity.Recorder.SetAudioEncoder(AudioEncoder.Default);
        //    mainActivity.Recorder.SetOutputFile(path);
        //    mainActivity.Recorder.Prepare();
        //    mainActivity.Recorder.Start();
        //}

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
    }
     

    //class DatachangedEventListener : Java.Lang.Object,IValueEventListener
    //{
    //    FirebaseClient firebase;
    //    Action<List<MessageContent>> handler;

    //    public DatachangedEventListener(FirebaseClient firebase, Action<List<MessageContent>> handler)
    //    {
    //        this.firebase = firebase;
    //        this.handler = handler;
    //    }

    //    public IntPtr Handle => this.Handle;

    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void OnCancelled(DatabaseError error)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async void OnDataChange(DataSnapshot snapshot)
    //    {
    //        await DisplayChatMessage();
    //    }

    //    private async System.Threading.Tasks.Task DisplayChatMessage()
    //    {
    //        var items = await firebase.Child("chats")
    //            .OnceAsync<MessageContent>();

    //        handler(items.Select(x => new MessageContent {
    //            Email = x.Object.Email,
    //            Message = x.Object.Message,
    //            Phone = x.Object.Phone
    //        }).ToList());
    //    }
    //}

    ////class OnRegisterFinishListener : IOnCompleteListener
    ////{
    ////    public IntPtr Handle => throw new NotImplementedException();

    ////    public void Dispose()
    ////    {
    ////        throw new NotImplementedException();
    ////    }

    ////    public void OnComplete(Task task)
    ////    {
    ////        if (task.IsSuccessful)
    ////        {
    ////            Toast.MakeText(, "SighInSucsessfully!", ToastLength.Short);
    ////        }
    ////    }
    ////}
}