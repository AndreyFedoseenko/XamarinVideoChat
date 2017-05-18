using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;
using Firebase.Database;
using Firebase.Xamarin.Database;
using System.Collections.Generic;
using System.Linq;
using Plugin.MediaManager.Forms.Android;
using Android.Media;
using System.Threading.Tasks;
using OpenToxServer;
using Com.Opentok.Android;
using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;

namespace DeviceSpecificApp.Droid
{
    [Activity(Label = "DeviceSpecificApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IValueEventListener, Session.ISessionListener, Publisher.IPublisherListener, Subscriber.IVideoListener
    {
        public FirebaseClient firebase;
        public Action<List<MessageContent>> handler;
        public MediaRecorder Recorder { get; set; }

        private NetworkProvider networkProvider;

        private Session _session;
        private Publisher _publisher;
        private Subscriber _subscriber;
        private List<Com.Opentok.Android.Stream> _streams = new List<Com.Opentok.Android.Stream>();

        private LinearLayout _publisherViewContainer;
        private LinearLayout _subscriberViewContainer;
        private LinearLayout.LayoutParams _subscriberLayoutParams;
        // Spinning wheel for loading subscriber view
        private ProgressBar _loadingSub;
        private bool isPublished;
        private static MainActivity instance = null;
        public MobileServiceClient MobileClient { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            var options = new FirebaseOptions.Builder()
            .SetApplicationId("simplemessager")
            .SetApiKey("AIzaSyCg847Wo53-3PLgUpkMWNpda9yL3qR-7HQ")
            .SetDatabaseUrl("https://simplemessager.firebaseio.com/")
            // .SetGcmSenderId(GcmSenderId)
            .Build();
            instance = this;

            FirebaseApp.InitializeApp(this, options);
            VideoViewRenderer.Init();
            // QuickbloxPlatform.Init();
            global::Xamarin.Forms.Forms.Init(this, bundle);

            var layoutParams = new LinearLayout.LayoutParams(
                       Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);

            this.networkProvider = new NetworkProvider();

            this.networkProvider.GetSessionInfo();

            this.MobileClient = new MobileServiceClient(AppValues.BaseServerUrl);

            isPublished = false;

            LoadApplication(new App());

            try
            {
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                System.Diagnostics.Debug.WriteLine("Registering...");

                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client.Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (!IsFinishing)
                return;

            isPublished = false;
            this.DisconnectFromStream();
        }

        public override void OnBackPressed()
        {
            this.DisconnectFromStream();
            isPublished = false;
            base.OnBackPressed();
        }

        public void SetLayouts(LinearLayout publiserLayout, LinearLayout subscriberLayout, ProgressBar loadingProggressBar)
        {
            this._publisherViewContainer = publiserLayout;
            this._subscriberViewContainer = subscriberLayout;
            this._loadingSub = loadingProggressBar;
            this._subscriberLayoutParams = new LinearLayout.LayoutParams(
           Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
        }

        public void OnCancelled(DatabaseError error)
        {
        }

        public async void OnDataChange(DataSnapshot snapshot)
        {
            await DisplayChatMessage();
        }

        public async System.Threading.Tasks.Task DisplayChatMessage()
        {
            try
            {
                var items = await firebase.Child("chats")
                    .OnceAsync<MessageContent>();

                if(items.Count > 0 && handler != null)
                {
                    handler(items.Select(x => new MessageContent
                    {
                        Email = x.Object.Email,
                        Message = x.Object.Message,
                        Phone = x.Object.Phone
                    }).ToList());
                }
            }
            catch(Exception ex)
            {
                var z = ex.Message;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Recorder != null)
            {
                Recorder.Release();
                Recorder.Dispose();
                Recorder = null;
            }
        }

        public async Task VideoCallSomeOne()
        {
            if (this._publisher != null)
                return;

            this._publisher = new Publisher(this, "publisher");
            this._publisher.SetPublisherListener(this);
            AttachPublisherView(this._publisher);
            this._session.Publish(this._publisher);
            isPublished = true;
        }

        public async Task ConnectToSession()
        {
            // var session = await this.networkProvider.GetSessionInfo();
            var session = new SessionInfo()
            {
                ApiKey = 45842692,
                SessionId = "2_MX40NTg0MjY5Mn5-MTQ5NDkyNzQ5MjYyM35wcGMybmNyQjJqRDNLSUJzbURLc2JtTEt-UH4",
                Token = "T1==cGFydG5lcl9pZD00NTg0MjY5MiZzaWc9M2E2ZTE5YTU4YTAzMmNkNDUwODliNjI5ODlmNjExMjU3MDY4YzBlMTpzZXNzaW9uX2lkPTJfTVg0ME5UZzBNalk1TW41LU1UUTVORGt5TnpRNU1qWXlNMzV3Y0dNeWJtTnlRakpxUkROTFNVSnpiVVJMYzJKdFRFdC1VSDQmY3JlYXRlX3RpbWU9MTQ5NDkyNzYzNSZub25jZT0wLjM2NzI5MzU5MzE5MzUwNTk2JnJvbGU9cHVibGlzaGVyJmV4cGlyZV90aW1lPTE0OTQ5NDkyMzM="
            };
            this.SessionConnect(session);
        }

        public void RejectVideoCall()
        {
            this.DisconnectFromStream();
        }

        private void SessionConnect(SessionInfo sessionInfo)
        {
            if (this._session != null)
                return;

            this._session = new Session(this, sessionInfo.ApiKey.ToString(), sessionInfo.SessionId);
            this._session.SetSessionListener(this);
            this._session.Connect(sessionInfo.Token);
        }

        public void DisconnectFromStream()
        {
            if (_session != null)
            {
                _session.Disconnect();
            }
        }

        public void OnConnected(Session p0)
        {
        }

        public void OnDisconnected(Session p0)
        {
            if (this._publisher != null)
            {
                this._publisherViewContainer.RemoveView(this._publisher.View);
            }

            if (this._subscriber != null)
            {
                this._subscriberViewContainer.RemoveView(this._subscriber.View);
            }

            this._publisher = null;
            this._subscriber = null;
            this._streams.Clear();
            this._session = null;
            if (isPublished)
            {
                isPublished = false;
            }
        }

        public void OnError(PublisherKit p0, OpentokError p1)
        {
        }

        public void OnError(Session p0, OpentokError p1)
        {
        }

        public void OnStreamCreated(PublisherKit p0, Com.Opentok.Android.Stream p1)
        {
            this._streams.Add(p1);
            //if (this._subscriber == null)
            //{
            //    SubscribeToStream(p1);
            //}
        }

        public void OnStreamDestroyed(PublisherKit p0, Com.Opentok.Android.Stream p1)
        {
            if ((this._subscriber != null))
            {
                UnsubscribeFromStream(p1);
            }
        }

        public void OnStreamDropped(Session p0, Com.Opentok.Android.Stream p1)
        {
            if (this._subscriber != null)
            {
                UnsubscribeFromStream(p1);
            }
        }

        public void OnStreamReceived(Session p0, Com.Opentok.Android.Stream p1)
        {
            this._streams.Add(p1);
            if (this._subscriber == null)
            {
                SubscribeToStream(p1);
            }
        }

        public void OnVideoDataReceived(SubscriberKit p0)
        {
            this._loadingSub.Visibility = ViewStates.Gone;
            AttachSubscriberView(this._subscriber);
        }

        public void OnVideoDisabled(SubscriberKit p0, string p1)
        {
        }

        public void OnVideoDisableWarning(SubscriberKit p0)
        {
        }

        public void OnVideoDisableWarningLifted(SubscriberKit p0)
        {
        }

        public void OnVideoEnabled(SubscriberKit p0, string p1)
        {
        }

        private void AttachSubscriberView(Subscriber subscriber)
        {
            var layoutParams = new LinearLayout.LayoutParams(
           Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
            this._subscriberViewContainer.AddView(this._subscriber.View, layoutParams);
            this._subscriber.SetStyle(BaseVideoRenderer.StyleVideoScale,
                BaseVideoRenderer.StyleVideoFill);
        }

        private void AttachPublisherView(Publisher publisher)
        {
            this._publisher.SetStyle(BaseVideoRenderer.StyleVideoScale, BaseVideoRenderer.StyleVideoFill);
            var layoutParams = new RelativeLayout.LayoutParams(320, 240);
            layoutParams.AddRule(LayoutRules.AlignParentBottom, -1);
            layoutParams.AddRule(LayoutRules.AlignParentRight, -1);
            this._publisherViewContainer.AddView(publisher.View, layoutParams);
        }

        private void SubscribeToStream(Com.Opentok.Android.Stream stream)
        {
            this._subscriber = new Subscriber(this, stream);
            this._subscriber.SetVideoListener(this);
            this._session.Subscribe(_subscriber);
            // start loading spinning
            this._loadingSub.Visibility = ViewStates.Visible;
        }

        private void UnsubscribeFromStream(Com.Opentok.Android.Stream stream)
        {
            this._streams.Remove(stream);
            if (this._subscriber.Stream.StreamId.Equals(stream.StreamId))
            {
                this._subscriberViewContainer.RemoveView(this._subscriber.View);
                this._subscriber = null;
                if (this._streams.Count != 0)
                {
                    SubscribeToStream(this._streams[0]);
                }
            }
        }

        public void OnVideoDisabled(SubscriberKit p0)
        {
        }

        public static MainActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }

        private void CreateAndShowDialog(string message, string title)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}

