using DeviceSpecificApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeviceSpecificApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        IDealer dialer;

        public string ChatName { get; set; }

        ObservableCollection<MessageInfo> messages;

        public ChatPage(string chatName)
        {
            InitializeComponent();
            messages = new ObservableCollection<MessageInfo>();
            MessagesssList.ItemsSource = messages;
            //this.dialer = DependencyService.Get<IDealer>();
            //dialer.handleAddingMessages(HandleChangeData);
            //dialer.DisplayChatMessage();
            this.ChatName = chatName;
        }

        protected override async void OnAppearing()
        {
            var messagesList = await App.NetworkProvider.GetMessages(this.ChatName);
            messages = new ObservableCollection<MessageInfo>(messagesList);
            MessagesssList.ItemsSource = messages;
        }

        public void MessageReceived(MessageInfo message)
        {
            messages.Add(message);
        }

        private void HandleChangeData(List<MessageInfo> mess)
        {
            MessagesssList.ItemsSource = mess;
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
            //await dialer.SendMessage(MessagesEntry.Text);
            var isSended = await App.NetworkProvider.SendMessage(this.ChatName, MessagesEntry.Text);
            if (isSended)
            {
                if (App.MobileClient.CurrentUser != null
                    && !string.IsNullOrEmpty(App.MobileClient.CurrentUser.MobileServiceAuthenticationToken))
                {
                    messages.Add(new MessageInfo()
                    {
                        SenderEmail = App.MobileClient.CurrentUser.UserId,
                        Text = MessagesEntry.Text,
                        ChatName = this.ChatName
                    });
                }
                MessagesEntry.Text = "";
            }
        }

        private async void InvitePerson(object sender, EventArgs e)
        {
            //await dialer.SendMessage(MessagesEntry.Text);
            var isInvited = await App.NetworkProvider.InvitePerson(this.ChatName, InvitationEntry.Text);
            if (isInvited)
            {
                InvitationEntry.Text = "";
            }
        }
    }
}
