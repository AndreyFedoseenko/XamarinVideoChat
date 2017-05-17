using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DeviceSpecificApp
{
    public partial class MainPage : ContentPage
    {
        IDealer dialer;

        public MainPage()
        {
            InitializeComponent();
            MessagesssList.ItemsSource = new List<MessageContent>();
            this.dialer = DependencyService.Get<IDealer>();
            dialer.handleAddingMessages(HandleChangeData);
            dialer.DisplayChatMessage();
        }

        private void HandleChangeData(List<MessageContent> mess)
        {
            MessagesssList.ItemsSource = mess;
        }

        private async void SendMessage_Clicked(object sender, EventArgs e)
        {
            await dialer.SendMessage(MessagesEntry.Text);
            MessagesEntry.Text = "";
        }

        private void Call_Clicked(object sender, EventArgs e)
        {
            var number = ((Button)sender).Text;
            var result = dialer.Call(number);
        }

        private async void Video_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new VideoPage());
        }

        private async void MessagesEntry_TextSended(object sender, EventArgs e)
        {
            await dialer.SendMessage(MessagesEntry.Text);
            MessagesEntry.Text = "";
        }
    }
}
