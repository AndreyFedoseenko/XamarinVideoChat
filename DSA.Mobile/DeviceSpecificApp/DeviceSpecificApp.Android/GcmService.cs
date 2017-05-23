using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Android.App;
using Android.Media;
using Android.Content;
using Android.Support.V4.App;
using Android.Util;
using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Android.Widget;
using DeviceSpecificApp.Model;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permissin.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
namespace DeviceSpecificApp.Droid
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDER_IDS = new string[] { "541610880089" };
    }

    [Service]
    public class GcmService:GcmServiceBase
    {
        public static string RegistrationID { get; private set; }

        public GcmService() : base(PushHandlerBroadcastReceiver.SENDER_IDS)
        {

        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info("PushHandlerBroadcastReceiver", "GCM Received!");

            var msg = new StringBuilder();

            if(intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                {
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
                }
            }

            var prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            var edit = prefs.Edit();
            edit.PutString("last_msg", msg.ToString());
            edit.Commit();

            var isInvitation = intent.Extras.GetString("isInvitation");
            var sender = intent.Extras.GetString("sender");
            var chat = intent.Extras.GetString("chat");

            if (!string.IsNullOrEmpty(isInvitation) && Convert.ToBoolean(isInvitation))
            {
                CreateInvitationNotification(sender, chat);
                return;
            }

            var text = intent.Extras.GetString("text");
            var message = new MessageInfo()
            {
                SenderEmail = sender,
                ChatName = chat,
                Text = text
            };
            App.ReceiveMessage(message);

        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "GCM Error: " + errorId);
        }
        protected override bool OnRecoverableError(Context context, string errorId)
        {
            return true;
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose("PushHandlerBroadcastReceiver", "GSM Registered: " + registrationId);
            RegistrationID = registrationId;

            var push = App.MobileClient.GetPush();

            MainActivity.CurrentActivity.RunOnUiThread(() => Register(push,null));
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "Unregistered Registeration Id: " + registrationId);
        }

        public async void Register(Push push,IEnumerable<string> tags)
        {
            try
            {
                const string templateBodyGCM = "{\"data\":{\"sender\":\"$(senderParam)\",\"chat\":\"$(chatParam)\", \"receiver\":\"$(receiverParam)\", \"isInvitation\":\"$(isInvitationParam)\", \"text\":\"$(textParam)\" }}";

                var templates = new JObject();
                templates["genericMessage"] = new JObject
                {
                    {"body",templateBodyGCM }
                };

                await push.RegisterAsync(RegistrationID, templates);
                Log.Info("Push Installation Id",push.InstallationId.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
        }

        void CreateInvitationNotification(string sender, string chat)
        {
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            var uiIntent = new Intent(this,typeof(MainActivity));

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this);

            string title = "Invitation!";
            var remoteView = new RemoteViews(MainActivity.CurrentActivity.PackageName, Resource.Layout.custom_notification);
            remoteView.SetImageViewResource(Resource.Id.notificationImage, Resource.Drawable.icon);
            remoteView.SetTextViewText(Resource.Id.title, title);
            remoteView.SetTextViewText(Resource.Id.text, string.Format("{0} invite you to chat: {1}!",sender,chat));

            Intent acceptIntent = new Intent(MainActivity.CurrentActivity, MainActivity.CurrentActivity.Receiver.Class);
            acceptIntent.PutExtra("sender", sender);
            acceptIntent.PutExtra("chat", chat);
            PendingIntent pendingAcceptIntent = PendingIntent.GetBroadcast(MainActivity.CurrentActivity, 0,
            acceptIntent, 0);

            remoteView.SetOnClickPendingIntent(Resource.Id.accept, pendingAcceptIntent);

            var notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 0, uiIntent, 0))
                .SetSmallIcon(Android.Resource.Drawable.SymActionEmail)
                .SetTicker(title)
                .SetContentTitle(title)
                .SetCustomContentView(remoteView)
                .SetContentIntent(null)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(true).Build();


        var remView = notification.ContentView;
            var btn = new Button(MainActivity.CurrentActivity);
            remView.SetOnClickPendingIntent(btn.Id, null);

            notificationManager.Notify(1, notification);
        }
    }
}