using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Plugin.MediaManager.Forms.iOS;
using DeviceSpecificApp.iOS.Providers;

namespace DeviceSpecificApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public static AppDelegate SelfDelegate { get; private set; }

        public VideoChatProvider VideoProvider {get; private set;}
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //var options = new FirebaseOptions.Builder()
            //.SetApplicationId("simplemessager")
            //.SetApiKey("AIzaSyCg847Wo53-3PLgUpkMWNpda9yL3qR-7HQ")
            //.SetDatabaseUrl("https://simplemessager.firebaseio.com/")
            //// .SetGcmSenderId(GcmSenderId)
            //.Build();

            //var firebaseOptions = new Firebase.Analytics.Options("//GoogleService-Info.plist");

            // RegisterDeal.mainActivity = this;

            Firebase.Analytics.App.Configure();

            VideoViewRenderer.Init();
            // QuickbloxPlatform.Init();

            global::Xamarin.Forms.Forms.Init();

            AppDelegate.SelfDelegate = this;

            VideoProvider = new VideoChatProvider()
            {
                SubscribeToSelf = false
            };
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
