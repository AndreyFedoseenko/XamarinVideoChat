using DeviceSpecificApp.Model;
using DeviceSpecificApp.Providers;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DeviceSpecificApp
{
    public partial class App : Application
    {
        public static NetworkProvider NetworkProvider { get; private set; }

        public static MobileServiceClient MobileClient { get; private set; }

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            var dialer = DependencyService.Get<IDealer>();
            NetworkProvider = new NetworkProvider();
            MobileClient = new MobileServiceClient(AppValues.BaseServerUrl);
            //try
            //{

            //    if (dialer != null)
            //    {
            //        // MainPage = dialer.IsAuthentificated() ? new NavigationPage(new MainPage()) : new NavigationPage(new SighIn());
            //        dialer.SetUpMessageHandler(InviteUserToChat);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var z = ex.Message;
            //}
            this.MainPage = new NavigationPage(new SighIn());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static Page CurrPage
        {
            get
            {
                if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
                {
                    int index = Application.Current.MainPage.Navigation.NavigationStack.Count - 1;
                    return Application.Current.MainPage.Navigation.NavigationStack[index];
                }
                return null;
            }
        }

        public static void ReceiveMessage(MessageInfo message)
        {
            var currP = CurrPage as ChatPage;
            if (currP != null)
            {
                if (currP.ChatName == message.ChatName)
                {
                    currP.MessageReceived(message);
                }
            }
        }
    }
}
