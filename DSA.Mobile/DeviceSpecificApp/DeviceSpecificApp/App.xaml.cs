using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DeviceSpecificApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            var dialer = DependencyService.Get<IDealer>();

            try
            {

                if (dialer != null)
                {
                    // MainPage = dialer.IsAuthentificated() ? new NavigationPage(new MainPage()) : new NavigationPage(new SighIn());
                    this.MainPage = new NavigationPage(new SighIn());
                }
            }
            catch(Exception ex)
            {
                var z = ex.Message;
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
