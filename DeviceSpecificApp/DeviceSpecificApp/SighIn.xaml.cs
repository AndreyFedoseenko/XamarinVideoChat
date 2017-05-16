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
        public SighIn()
        {
            InitializeComponent();
        }

        private async Task RegisterButton_Clicked(object sender, EventArgs e)
        {
            var dialer = DependencyService.Get<IDealer>();
            // await dialer.Register(emailEntry.Text, passwordEntry.Text);
            await this.Navigation.PushAsync(new MainPage());
        }
    }
}
