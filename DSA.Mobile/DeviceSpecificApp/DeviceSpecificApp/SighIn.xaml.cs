using DeviceSpecificApp.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeviceSpecificApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SighIn : ContentPage
    {
        private AuthentificationProvider provider;
        public SighIn()
        {
            InitializeComponent();
            provider = new AuthentificationProvider();
        }

        private async Task RegisterButton_Clicked(object sender, EventArgs e)
        {
            var dialer = DependencyService.Get<IDealer>();
            var registration = dialer.GetRegistrationId();
            
            var isRegistered = await provider.Authentificate(emailEntry.Text, passwordEntry.Text, App.MobileClient.InstallationId);
            if (isRegistered)
            {
                this.ValidationLabel.IsVisible = false;
                await this.Navigation.PushAsync(new MainPage());
            }
            else
            {
                this.ValidationLabel.IsVisible = true;
            }
        }
    }
}
