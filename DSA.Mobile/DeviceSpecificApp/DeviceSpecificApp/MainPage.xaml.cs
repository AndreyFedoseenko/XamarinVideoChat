using DeviceSpecificApp.Model;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DeviceSpecificApp
{
    public partial class MainPage : ContentPage
    {
        IDealer dialer;

        ObservableCollection<string> chats;

        public static MainPage Instance { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            chats = new ObservableCollection<string>();
            ChatList.ItemSelected += ChatChoiced;
            Instance = this;
        }

        public async Task AcceptInvitation(string sender, string chat)
        {
            var isAccepted = await App.NetworkProvider.AcceptInvitation(chat, sender);
            if (isAccepted)
            {
                chats.Add(chat);
                ChatList.ItemsSource = chats;
            }
        }

        protected override async void OnAppearing()
        {
            var chatList = await App.NetworkProvider.GetChats();
            chats = new ObservableCollection<string>(chatList);
            ChatList.ItemsSource = chats;
        }

        private async void CreateChat(object sender, EventArgs e)
        {
            var isCreated = await App.NetworkProvider.CreateChat(ChatCreationEntry.Text);
            if (isCreated)
            {
                chats.Add(ChatCreationEntry.Text);
                ChatList.ItemsSource = chats;
                ChatCreationEntry.Text = "";
            }
        }

        private void ChatChoiced(object sender, SelectedItemChangedEventArgs e)
        {
            this.Navigation.PushAsync(new ChatPage(e.SelectedItem.ToString()));
        }
    }
}
